using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI scoreText;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI bestScoreText;
    public Button restartButton;
    public Button quitButton;
    public Button menuButton;

    private int _score;
    private bool _gameOver;

    private const string BestKey = "BestScore";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        _score = 0;
        _gameOver = false;
        UpdateHUD();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Подписываемся на кнопки
        if (restartButton != null) restartButton.onClick.AddListener(Restart);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        if (menuButton != null) menuButton.onClick.AddListener(ToMainMenu);
    }

    public void AddScore(int amount)
    {
        if (_gameOver) return;
        _score += amount;
        UpdateHUD();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayEat();
    }

    public void GameOver()
    {
        if (_gameOver) return;
        _gameOver = true;

        int best = PlayerPrefs.GetInt(BestKey, 0);
        if (_score > best)
        {
            best = _score;
            PlayerPrefs.SetInt(BestKey, best);
            PlayerPrefs.Save();
        }

        // Заполняем панель
        if (finalScoreText != null) finalScoreText.text = $"Score: {_score}";
        if (bestScoreText != null) bestScoreText.text = $"Best: {best}";

        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        Debug.Log($"Game Over! Score: {_score}, Best: {best}");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateHUD()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {_score}";
    }
}