using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseMinigame : MonoBehaviour
{
    [SerializeField] protected int maxScore;
    protected float curScore = 0;
    public Attribute attribute;
    public GameObject gameCanvas;
    public GameObject tutorial;

    [SerializeField] protected float gameDuration;
    protected float startTime;
    private LinearTimer linearTimer;
    [SerializeField]
    private TextMeshProUGUI score;

    public virtual void GameOver()
    {
        gameCanvas.SetActive(false);
        // Stop minigame music and resume background music
        AudioManager.Instance.EndMinigame();
        MinigameManager.instance.GameScore(curScore, maxScore, attribute);
        Time.timeScale = 1f;
        enabled = false;
    }

    public virtual void StartTutorial()
    {
        Time.timeScale = 0f;
        curScore = 0;
        score.text = curScore.ToString();
        gameCanvas.SetActive(true);

        if (tutorial)
        {
            tutorial.SetActive(true);
        }
        else
        {
            StartGame();
        }
    }

    public virtual void StartGame()
    {
        tutorial.SetActive(false);
        startTime = Time.realtimeSinceStartup;
        linearTimer = FindObjectOfType<LinearTimer>();
        linearTimer.StartTimer(gameDuration);

        // subscribes a function to timer event
        linearTimer.OnTimerEnd += GameOver;
    }

    protected void AddScore(int scoreToAdd)
    {
        if (!(curScore == 0 && scoreToAdd < 0))
        {
            curScore += scoreToAdd;
            score.text = curScore.ToString();
        }
    }

    protected virtual void GameInput() { }
}
