using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class SnakeController : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform segmentsParent;      // объект "Segments"
    public GameObject segmentPrefab;      // префаб сегмента
    public GameObject headPrefab;

    [Header("Настройки")]
    public int startLength = 3;
    public Vector2Int startCell = new Vector2Int(10, 10);
    public Vector2Int startDirection = Vector2Int.right;

    [Header("Тайминг")]
    public float stepInterval = 0.15f;    // секунд на 1 клетку
    public float minStepInterval = 0.07f;
    public float speedUpEvery = 5f;       // каждые N яблок ускоряемся
    public float speedUpAmount = 0.005f;

    // Логика
    private readonly List<Vector2Int> _cells = new();       // [0] — голова
    private readonly List<Transform> _segments = new();
    private Vector2Int _currentDirection;
    private Vector2Int _queuedDirection;
    private float _timer;
    private int _applesEaten;
    private bool _alive = true;

    public Vector2Int Head => _cells[0];
    public int Length => _cells.Count;
    public bool IsAlive => _alive;

    private void Start()
    {
        _currentDirection = startDirection;
        _queuedDirection = startDirection;

        // Инициализируем змейку: голова + хвост
        for (int i = 0; i < startLength; i++)
        {
            Vector2Int cell = startCell - _currentDirection * i;
            _cells.Add(cell);
            SpawnSegment(cell, isHead: i == 0);
        }

        // Ставим корень змейки в мировые координаты головы
        transform.position = GridManager.Instance.CellToWorld(_cells[0]);
    }

    private void Update()
    {
        if (!_alive) return;

        _timer += Time.deltaTime;
        if (_timer >= stepInterval)
        {
            _timer -= stepInterval;
            Step();
        }

        // Плавная интерполяция визуала (сегменты едут к своим клеткам)
        UpdateVisual();
    }

    private void Step()
    {
        // Применяем отложенный поворот
        if (_queuedDirection != -_currentDirection)
            _currentDirection = _queuedDirection;

        Vector2Int newHead = _cells[0] + _currentDirection;

        // Стена?
        if (!GridManager.Instance.IsInside(newHead)) { Die(); return; }

        // Своё тело? (хвост не считаем, если не растём — он уедет)
        // Для простоты: всегда проверяем все клетки кроме последней
        for (int i = 0; i < _cells.Count - 1; i++)
            if (_cells[i] == newHead) { Die(); return; }

        // Двигаем
        _cells.Insert(0, newHead);

        // Если съели еду — не удаляем хвост (рост)
        if (FoodSpawner.Instance != null && FoodSpawner.Instance.TryEat(newHead))
        {
            _applesEaten++;
            SpawnSegment(_cells[_cells.Count - 1]);
            GameManager.Instance.AddScore(1);

            if (_applesEaten % speedUpEvery == 0)
                stepInterval = Mathf.Max(minStepInterval, stepInterval - speedUpAmount);
        }
        else
        {
            _cells.RemoveAt(_cells.Count - 1);
        }
    }

    private void UpdateVisual()
    {
        // Мягко двигаем трансформ корня к голове
        Vector3 target = GridManager.Instance.CellToWorld(_cells[0]);
        transform.position = Vector3.Lerp(transform.position, target, 0.5f);

        var head = _segments[0];
        float angle = Mathf.Atan2(_currentDirection.y, _currentDirection.x) * Mathf.Rad2Deg;
        head.rotation = Quaternion.Euler(0, 0, angle - 90f);  // если спрайт смотрит вверх
                                                              // или angle, если спрайт смотрит вправо

        // Сегменты — плавно к своим клеткам
        float t = 1f - Mathf.Exp(-20f * Time.deltaTime);
        for (int i = 0; i < _segments.Count; i++)
        {
            Vector3 pos = GridManager.Instance.CellToWorld(_cells[i]);
            _segments[i].position = Vector3.Lerp(_segments[i].position, pos, t);
        }
    }

    public bool OccupiesCell(Vector2Int cell)
    {
        for (int i = 0; i < _cells.Count; i++)
            if (_cells[i] == cell) return true;
        return false;
    }

    private void SpawnSegment(Vector2Int cell, bool isHead = false)
    {
        GameObject prefab = isHead ? headPrefab : segmentPrefab;
        GameObject go = Instantiate(prefab, segmentsParent);
        go.transform.position = GridManager.Instance.CellToWorld(cell);
        _segments.Add(go.transform);
    }

    public void SetDirection(Vector2Int dir)
    {
        if (!_alive) return;
        if (dir == -_currentDirection) return;   // нельзя назад
        _queuedDirection = dir;
    }

    private void Die()
    {
        _alive = false;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayDeath();
        GameManager.Instance.GameOver();
    }
}