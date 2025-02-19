using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMinigame : MonoBehaviour
{
    [SerializeField] protected int maxScore;
    protected float curScore = 0;
    public Attribute attribute;
    public GameObject gameCanvas;

    [SerializeField] protected float gameDuration;
    protected float startTime;

    public virtual void GameOver()
    {
        gameCanvas.SetActive(false);
        MinigameManager.instance.GameScore(curScore, maxScore, attribute);
        curScore = 0;
        Time.timeScale = 1f;
        enabled = false;
    }

    public virtual void StartGame()
    {
        startTime = Time.realtimeSinceStartup;
        gameCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    protected virtual void GameInput() { }
}
