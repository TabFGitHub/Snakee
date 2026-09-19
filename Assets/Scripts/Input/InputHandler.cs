using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public SnakeController snake;

    private SnakeControls _controls;

    private void Awake()
    {
        _controls = new SnakeControls();
    }

    private void OnEnable()
    {
        // Игровое управление
        _controls.Gameplay.Enable();

        _controls.Gameplay.MoveUp.performed += ctx => snake.SetDirection(Vector2Int.up);
        _controls.Gameplay.MoveDown.performed += ctx => snake.SetDirection(Vector2Int.down);
        _controls.Gameplay.MoveLeft.performed += ctx => snake.SetDirection(Vector2Int.left);
        _controls.Gameplay.MoveRight.performed += ctx => snake.SetDirection(Vector2Int.right);

        // Пауза — отдельная карта, всегда активна
        _controls.UI.Enable();
        _controls.UI.TogglePause.performed += OnTogglePause;
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
        _controls.UI.Disable();

        _controls.UI.TogglePause.performed -= OnTogglePause;
    }

    private void OnTogglePause(InputAction.CallbackContext ctx)
    {
        if (PauseManager.Instance == null) return;
        PauseManager.Instance.TogglePause();
    }

    // Методы для PauseManager (способ B2)
    public void EnableGameplay() => _controls.Gameplay.Enable();
    public void DisableGameplay() => _controls.Gameplay.Disable();
}