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

    PhysicsScene2D physicsScene;

    public int curScore;
    public int maxScore;

    void Awake()
    {
        catchGame = FindObjectOfType<CatchGame>();
        linearTimer = FindObjectOfType<LinearTimer>();
        linearTimer.StartTimer(gameTime);
        // subscribe to timer event
        linearTimer.OnTimerEnd += HandleGameOver;
        gameOver = false;
        curScore = 0;
        // Start minigame music (ToeBeansMinigame is Minigame #1)
        AudioManager.Instance.StartMinigame(1);
    }
    void Start()
    {
        gameUI.UpdateScoreUI(curScore);
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

    void HandleGameOver()
    {
        gameOver = true;
        catchGame.GameOver();

        // Stop minigame music and resume background music
        AudioManager.Instance.EndMinigame();
        Debug.Log("Ended Game Music");

    }

}
