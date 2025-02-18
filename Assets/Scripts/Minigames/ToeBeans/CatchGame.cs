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
        StartCoroutine(UnloadGameCoroutine());
    }

    public override void StartGame()
    {
        Time.timeScale = 0f;
        gameCanvas.SetActive(true);
        StartCoroutine(StartGameCoroutine());
        enabled = true;
        Physics2D.simulationMode = SimulationMode2D.Script;
    }

    private IEnumerator UnloadGameCoroutine()
    {
        string s = SceneManager.GetActiveScene().name;
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(s);
        yield return new WaitUntil(() => asyncUnload.isDone);
        Debug.Log("Unloaded Scene: " + SceneManager.GetActiveScene().name);
        yield return null;
        yield return null;
        curScore = 0;
        gameCanvas.SetActive(false);
        Time.timeScale = 1f;
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        enabled = false;
        Debug.Log("Scene Game Over");
    }

    private IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

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
            toeBeansMinigame.maxScore = maxScore;
        }
        else
        {
            Debug.LogError("ToeBeansMinigame script not found in scene!");
        }
    }
}
