using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class ToeBeansMinigame : MonoBehaviour
{
    public bool gameOver = false;
    private float timer = 0f;
    public float gameTime = 60f;

    public ToeBeansUI gameUI;

    private CatchGame catchGame;

    public int curScore;
    public int maxScore;

    void Awake()
    {
        catchGame = FindObjectOfType<CatchGame>();
        ResetGame();
    }
    void Start()
    {
        gameUI.UpdateScoreUI(curScore);
        gameUI.UpdateTimerUI((int)gameTime);
    }

    public void AddScore(int scoreToAdd)
    {
        if (gameOver) return; // Prevent adding score after game over

        curScore += scoreToAdd;
        gameUI.UpdateScoreUI(curScore);
    }

    void Update()
    {
        if (!gameOver)
        {
            timer += Time.unscaledDeltaTime;
            int timeRemaining = Mathf.Max(0, (int)(gameTime - timer));
            gameUI.UpdateTimerUI(timeRemaining);

            if (timer >= gameTime)
            {
                catchGame.GameOver();
            }
        }

        // Debug Reset
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }
    }

    void ResetGame()
    {
        gameOver = false;
        timer = 0;
        curScore = 0;

        gameUI.UpdateScoreUI(curScore);
        gameUI.UpdateTimerUI((int)gameTime);
    }

}
