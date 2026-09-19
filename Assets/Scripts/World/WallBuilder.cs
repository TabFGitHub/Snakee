using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    [Header("Ссылки")]
    public GameObject wallSegmentPrefab;
    public Transform wallsParent;

    [Header("Настройки")]
    [Tooltip("Толщина стен в клетках (1 = одна линия по периметру)")]
    public int thickness = 1;

    private void Start()
    {
        BuildWalls();
    }

    private void BuildWalls()
    {
        var grid = GridManager.Instance;

        // Верхняя и нижняя границы (по ширине поля)
        for (int x = -thickness; x < grid.width + thickness; x++)
        {
            for (int t = 0; t < thickness; t++)
            {
                SpawnWall(new Vector2Int(x, grid.height + t));   // сверху
                SpawnWall(new Vector2Int(x, -1 - t));            // снизу
            }
        }

        // Левая и правая границы (по высоте поля, без дублирования углов)
        for (int y = 0; y < grid.height; y++)
        {
            for (int t = 0; t < thickness; t++)
            {
                SpawnWall(new Vector2Int(-1 - t, y));            // слева
                SpawnWall(new Vector2Int(grid.width + t, y));    // справа
            }
        }
    }

    private void SpawnWall(Vector2Int cell)
    {
        GameObject go = Instantiate(wallSegmentPrefab, wallsParent);
        // Для стен используем ту же формулу, что и для обычных клеток
        go.transform.position = GridManager.Instance.CellToWorld(cell);
        go.name = $"Wall_{cell.x}_{cell.y}";
    }
}