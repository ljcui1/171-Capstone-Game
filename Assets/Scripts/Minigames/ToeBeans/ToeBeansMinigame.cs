using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class ToeBeansMinigame : MonoBehaviour
{
    public bool gameOver = false;
    public float gameTime = 20f;

    public ToeBeansUI gameUI;
    private LinearTimer linearTimer;

    private CatchGame catchGame;

    public int curScore;
    public int maxScore;

    void Awake()
    {
        catchGame = FindObjectOfType<CatchGame>();
        linearTimer = FindObjectOfType<LinearTimer>();
        linearTimer.StartTimer(gameTime);

        // subscribe to timer event
        linearTimer.OnTimerEnd += HandleGameOver;
        ResetGame();

        // Start minigame music (ToeBeansMinigame is Minigame #1)
        AudioManager.Instance.StartMinigame(1);
    }
    void Start()
    {
        gameUI.UpdateScoreUI(curScore);
        Physics2D.simulationMode = SimulationMode2D.Script;
    }

    public void AddScore(int scoreToAdd)
    {
        if (gameOver) return; // Prevent adding score after game over
        if (!(curScore == 0 && scoreToAdd < 0))
        {
            curScore += scoreToAdd;
        }

        gameUI.UpdateScoreUI(curScore);
    }

    void Update()
    {
        // Debug Reset
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }

        Physics2D.Simulate(Time.unscaledDeltaTime);
    }

    void ResetGame()
    {
        gameOver = false;
        curScore = 0;
        linearTimer.StartTimer(gameTime);
        // subscribe to timer event
        linearTimer.OnTimerEnd += HandleGameOver;
        gameUI.UpdateScoreUI(curScore);
    }

    void HandleGameOver()
    {
        gameOver = true;
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        catchGame.GameOver();

        // Stop minigame music and resume background music
        AudioManager.Instance.EndMinigame();

    }

}
