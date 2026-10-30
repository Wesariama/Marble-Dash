using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    public GameView view;

    private GameModel model;

    void Start()
    {
        // Создаём модель
        model = new GameModel(3);

        // Подписываемся на события модели
        model.OnLivesChanged += UpdateLives;
        model.OnScoreChanged += UpdateScore;
        model.OnGameOver += HandleGameOver;

        // Кнопка рестарта — View сообщает Presenter'у, что её нажали
        view.restartButton.onClick.AddListener(HandleRestart);
        view.menuButton.onClick.AddListener(BackToMenu);

        // Первичная отрисовка
        UpdateLives();
        UpdateScore();
        view.HideGameOver();
    }

    // --- Методы, которые вызывают другие системы игры ---

    public void AddScore(int amount)
    {
        model.AddScore(amount);
    }

    public void PlayerTakeDamage()
    {
        model.LoseLife();
    }

    // --- Реакция на события модели ---

    void UpdateLives() => view.SetLives(model.Lives);
    void UpdateScore() => view.SetScore(model.Score);

    void HandleGameOver()
    {
        view.ShowGameOver();
        Time.timeScale = 0f;
    }

    void HandleRestart()
    {
        Time.timeScale = 1f;
        model.Reset();
        view.HideGameOver();
        // Здесь позже — перезагрузка сцены или сброс шарика и кубиков
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
