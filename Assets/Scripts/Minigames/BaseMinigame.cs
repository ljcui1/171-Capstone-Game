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
        curScore = 0;
        Time.timeScale = 1f;
        enabled = false;
    }

    public virtual void StartGame()
    {
        score.text = curScore.ToString();
        startTime = Time.realtimeSinceStartup;
        gameCanvas.SetActive(true);
        Time.timeScale = 0f;

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
