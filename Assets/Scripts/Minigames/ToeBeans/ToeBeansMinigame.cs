using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class ToeBeansMinigame : BaseMinigame
{
    public bool gameOver = false;
    private float timer = 0f;
    public float gameTime = 60f;

    public ToeBeansUI gameUI;

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
            timer += Time.deltaTime;
            int timeRemaining = Mathf.Max(0, (int)(gameTime - timer));
            gameUI.UpdateTimerUI(timeRemaining);

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

    void ResetGame()
    {
        gameOver = false;
        timer = 0;
        curScore = 0;

        gameUI.UpdateScoreUI(curScore);
        gameUI.UpdateTimerUI((int)gameTime);
    }

    // Scene stuff inspired by https://gist.github.com/kurtdekker/862da3bc22ee13aff61a7606ece6fdd3
    public override void GameOver()
    {
        string s = SceneManager.GetActiveScene().name;
        Debug.Log("Unloaded Scene: " + SceneManager.GetActiveScene().name);
        SceneManager.UnloadSceneAsync(s);
        MinigameManager.instance.GameScore(curScore, maxScore, attribute);
        curScore = 0;
        Time.timeScale = 1f;
        enabled = false;
    }

    public override void StartGame()
    {
        Time.timeScale = 0f;
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        Time.timeScale = 0f;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ToeBeansMinigame", LoadSceneMode.Additive);
        yield return asyncLoad; // Wait until the scene is loaded
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("ToeBeansMinigame"));
        Debug.Log("Active Scene: " + SceneManager.GetActiveScene().name);
    }
}
