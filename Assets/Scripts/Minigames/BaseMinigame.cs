using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMinigame : MonoBehaviour
{
    [SerializeField] private int maxScore;
    private int curScore = 0;
    public Attribute attribute;
    public Canvas gameCanvas;

    private MinigameManager manager = MinigameManager.instance;

    public virtual void GameOver()
    {
        gameCanvas.enabled = false;
        manager.GameScore(curScore, maxScore, attribute);
        curScore = 0;
        enabled = false;
    }

    public virtual void StartGame()
    {
        Time.timeScale = 0f;
        gameCanvas.enabled = true;
    }
}
