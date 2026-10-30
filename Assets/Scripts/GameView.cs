using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [Header("Ссылки на UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button menuButton;
    public GameObject GameOverScreen;

    // Методы для Presenter'а
    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void SetLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        if (GameOverScreen != null) GameOverScreen.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        if (GameOverScreen != null) GameOverScreen.SetActive(false);
    }
}