using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatchGame : BaseMinigame
{
    private ToeBeansMinigame toeBeansMinigame;
    // Scene stuff inspired by https://gist.github.com/kurtdekker/862da3bc22ee13aff61a7606ece6fdd3
    public override void GameOver()
    {
        curScore = toeBeansMinigame.curScore;
        MinigameManager.instance.GameScore(curScore, maxScore, attribute);
        string s = SceneManager.GetActiveScene().name;
        Debug.Log("Unloaded Scene: " + SceneManager.GetActiveScene().name);
        SceneManager.UnloadSceneAsync(s);
        curScore = 0;
        gameCanvas.SetActive(false);
        Time.timeScale = 1f;
        enabled = false;
    }

    public override void StartGame()
    {
        Time.timeScale = 0f;
        gameCanvas.SetActive(true);
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ToeBeansMinigame", LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone); // Ensure the scene is fully loaded

        Scene scene = SceneManager.GetSceneByName("ToeBeansMinigame");
        if (scene.IsValid())
        {
            SceneManager.SetActiveScene(scene); // Set the loaded scene as active
            // Debug.Log("Active Scene: " + SceneManager.GetActiveScene().name);
        }
        else
        {
            // Debug.LogError("Failed to find ToeBeansMinigame scene!");
            yield break; // Stop execution if scene wasn't found
        }

        // Wait a few frames to ensure objects are initialized
        yield return null;
        yield return null;

        toeBeansMinigame = FindObjectOfType<ToeBeansMinigame>();
        if (toeBeansMinigame != null)
        {
            // Debug.Log("ToeBeansMinigame found: " + toeBeansMinigame.name);
            toeBeansMinigame.maxScore = maxScore;
        }
        else
        {
            // Debug.LogError("ToeBeansMinigame script not found in scene!");
        }
    }
}
