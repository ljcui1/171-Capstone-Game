using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minigame2Manager : MonoBehaviour
{
    public int playerScore = 0;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI timerUI;

    public bool gameOver = false;
    private float timer = 0f;
    public float gameTime = 60f;

    void Start()
    {
        UpdateScoreUI();
        UpdateTimerUI();
    }

    public void AddScore(int scoreToAdd)
    {
        if (gameOver) return; // Prevent adding score after game over

        playerScore += scoreToAdd;
        UpdateScoreUI();
    }

    void Update()
    {
        if (!gameOver)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();

            if (timer >= gameTime)
            {
                GameOver();
            }
        }

        // Debug Reset
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        gameOver = true;
    }

    void ResetGame()
    {
        gameOver = false;
        timer = 0;
        playerScore = 0;

        UpdateScoreUI();
        UpdateTimerUI();
    }

    void UpdateScoreUI()
    {
        scoreUI.SetText(playerScore.ToString());
    }

    void UpdateTimerUI()
    {
        int timeRemaining = Mathf.Max(0, (int)(gameTime - timer));
        timerUI.SetText(timeRemaining.ToString());
    }
}
