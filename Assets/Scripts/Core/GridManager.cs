using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Размер поля в клетках")]
    public int width = 20;
    public int height = 20;

    [Header("Смещение поля (левый нижний угол)")]
    public Vector2 origin = new Vector2(0f, 0f);

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    /// <summary>Центр клетки в мировых координатах.</summary>
    public Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(origin.x + cell.x + 0.5f,
                           origin.y + cell.y + 0.5f,
                           0f);
    }

    /// <summary>Мировая позиция → клетка.</summary>
    public Vector2Int WorldToCell(Vector3 world)
    {
        return new Vector2Int(
            Mathf.FloorToInt(world.x - origin.x),
            Mathf.FloorToInt(world.y - origin.y));
    }

    public bool IsInside(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < width &&
               cell.y >= 0 && cell.y < height;
    }
}