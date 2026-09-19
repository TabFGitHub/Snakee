using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance { get; private set; }

    public GameObject foodPrefab;
    public SnakeController snake;

    private GameObject _currentFood;
    private Vector2Int _foodCell;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        var grid = GridManager.Instance;

        // Пытаемся найти свободную клетку (до 500 попыток)
        for (int attempt = 0; attempt < 500; attempt++)
        {
            var cell = new Vector2Int(
                Random.Range(0, grid.width),
                Random.Range(0, grid.height));

            if (IsFree(cell)) { PlaceFood(cell); return; }
        }
        // Если не нашли — победа (в реальной игре можно предусмотреть)
    }

    private bool IsFree(Vector2Int cell)
    {
        // Не на змейке (проверяем через публичное свойство при необходимости)
        // Для простоты — FoodSpawner не знает про клетки змейки,
        // поэтому попросим SnakeController вернуть занятые клетки.
        // См. следующий шаг — добавим метод GetCells() в SnakeController.
        return !snake.OccupiesCell(cell);
    }

    private void PlaceFood(Vector2Int cell)
    {
        if (_currentFood != null) Destroy(_currentFood);

        _foodCell = cell;
        _currentFood = Instantiate(foodPrefab, transform);
        _currentFood.transform.position = GridManager.Instance.CellToWorld(cell);
    }

    /// <summary>Проверяет, съела ли змейка еду в клетке. Если да — спавнит новую.</summary>
    public bool TryEat(Vector2Int cell)
    {
        if (cell != _foodCell) return false;

        // Спавним новую еду (после того как змейка сдвинется)
        // Небольшая задержка не нужна — просто вызовем
        Invoke(nameof(SpawnFood), 0f);
        _foodCell = new Vector2Int(-1, -1); // помечаем, что еды нет
        return true;
    }
}