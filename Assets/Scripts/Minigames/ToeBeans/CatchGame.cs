using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatchGame : BaseMinigame
{
    private Scene mainScene;
    private ToeBeansMinigame toeBeansMinigame;
    private PhysicsScene2D physics2DScene;
    private float physics2DSceneTimeScale = 1;
    // Scene stuff inspired by https://gist.github.com/kurtdekker/862da3bc22ee13aff61a7606ece6fdd3
    public override void GameOver()
    {
        curScore = toeBeansMinigame.curScore;
        MinigameManager.instance.GameScore(curScore, maxScore, attribute);
        StartCoroutine(UnloadGameCoroutine());
    }

    public override void StartGame()
    {
        mainScene = SceneManager.GetActiveScene();
        Time.timeScale = 0f;
        gameCanvas.SetActive(true);
        StartCoroutine(StartGameCoroutine());
        enabled = true;
        Physics2D.simulationMode = SimulationMode2D.Script;

        // Changes physics to be automated 
        // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics2D.Simulate.html
    }

    private void Update()
    {
        if (physics2DScene.IsValid())
        {
            physics2DScene.Simulate(Time.deltaTime * physics2DSceneTimeScale);
        }
    }

    private IEnumerator UnloadGameCoroutine()
    {
        string s = SceneManager.GetActiveScene().name;
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(s);
        Physics2D.SyncTransforms();
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        physics2DScene = default;
        yield return new WaitUntil(() => asyncUnload.isDone);
        if (mainScene.IsValid())
        {
            SceneManager.SetActiveScene(mainScene);
        }
        s = SceneManager.GetActiveScene().name;
        Debug.Log(s);
        // Debug.Log("Unloaded Scene: " + SceneManager.GetActiveScene().name);
        yield return null;
        yield return null;
        curScore = 0;
        gameCanvas.SetActive(false);
        Time.timeScale = 1f;


        enabled = false;
    }

    private IEnumerator StartGameCoroutine()
    {
        // , LocalPhysicsMode.Physics2D
        LoadSceneParameters param = new LoadSceneParameters(LoadSceneMode.Additive);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ToeBeansMinigame", param);
        yield return new WaitUntil(() => asyncLoad.isDone); // Ensure the scene is fully loaded

        Scene scene = SceneManager.GetSceneByName("ToeBeansMinigame");
        if (scene.IsValid())
        {
            SceneManager.SetActiveScene(scene); // Set the loaded scene as active
            physics2DScene = scene.GetPhysicsScene2D();
            Debug.Log("Active Scene: " + SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.LogError("Failed to find ToeBeansMinigame scene!");
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


