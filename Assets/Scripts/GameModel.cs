using System;

public class GameModel
{
    public int Lives { get; private set; }
    public int Score { get; private set; }
    public bool IsGameOver => Lives <= 0;

    // События для Presenter'а
    public event Action OnLivesChanged;
    public event Action OnScoreChanged;
    public event Action OnGameOver;

    public GameModel(int startLives)
    {
        Lives = startLives;
        Score = 0;
    }

    public void LoseLife()
    {
        if (IsGameOver) return;
        Lives--;
        OnLivesChanged?.Invoke();
        if (Lives <= 0) OnGameOver?.Invoke();
    }

    public void AddScore(int amount)
    {
        if (IsGameOver) return;
        Score += amount;
        OnScoreChanged?.Invoke();
    }

    public void Reset()
    {
        Lives = 3;
        Score = 0;
        OnLivesChanged?.Invoke();
        OnScoreChanged?.Invoke();
    }
}
