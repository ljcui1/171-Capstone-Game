using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minigame2Manager : MonoBehaviour
{
    public int playerScore;
    public TextMeshProUGUI score;
    public TextMeshProUGUI timerUI;

    public bool gameOver = false;
    public float timer = 0;

    public float gameTime = 60;

    public void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        score.SetText(playerScore.ToString());
    }

    void Update()
    {
        int timeRemaining = (int)(gameTime - timer);
        timerUI.SetText(timeRemaining.ToString());
        if (timer < gameTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Debug.Log("Game Over");
            gameOver = true;
        }

        // resets game when space is pressed - debug
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResetGame()
    {
        gameOver = false;
        timer = 0;
        playerScore = 0;
    }

}
