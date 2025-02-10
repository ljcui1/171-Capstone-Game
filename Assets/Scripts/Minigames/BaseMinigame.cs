using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMinigame : MonoBehaviour
{
    [SerializeField] private int maxScore;
    private int curScore = 0;
    public Attribute attribute;
    public GameObject gameCanvas;

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
        gameCanvas.SetActive(true);
        Time.timeScale = 0f;

    }
}
