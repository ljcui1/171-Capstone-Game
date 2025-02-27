using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiceGame : BaseMinigame
{
    //public Canvas mouseGame;
    [Header("Paw Images")]
    [SerializeField]
    private Image pawa;

    [SerializeField]
    private Image paws;

    [SerializeField]
    private Image pawd;

    [SerializeField]
    private Image pawf;

    [Header("Mice Images")]
    [SerializeField]
    private Image mouse1;

    [SerializeField]
    private Image mouse2;

    [SerializeField]
    private Image mouse3;

    [SerializeField]
    private Image mouse4;

    [Header("Game Variables")]
    [SerializeField]
    private float gameTime = 20f;

    [SerializeField]
    private bool gameOver = false;

    [SerializeField]
    private float curScore;

    [SerializeField]
    private float maxScore;

    [SerializeField]
    private LinearTimer linearTimer;

    [SerializeField]
    private float minTime;

    [SerializeField]
    private float maxTime;

    void Awake()
    {
        linearTimer = FindObjectOfType<LinearTimer>();
        linearTimer.StartTimer(gameTime);
        linearTimer.OnTimerEnd += HandleGameOver;
        ResetGame();
    }

    public override void StartGame()
    {
        base.StartGame();
        // Start minigame music (W.A.M. Minigame is Minigame #2)
        AudioManager.Instance.StartMinigame(2);
        pawa.enabled = true;
        paws.enabled = false;
        pawd.enabled = false;
        pawf.enabled = false;
        StartCoroutine(MousePop(mouse1));
        StartCoroutine(MousePop(mouse2));
        StartCoroutine(MousePop(mouse3));
        StartCoroutine(MousePop(mouse4));
    }

    // Update is called once per frame
    void Update()
    {
        GameInput();

        /*if (Time.realtimeSinceStartup - startTime > gameDuration)
        {
            GameOver();
        }*/
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }

        if (mouse1.enabled && pawa.enabled)
        {
            AudioManager.Instance.PlayThumpSound();
            AudioManager.Instance.PlaySqueakSound();
            AddScore(1);
            mouse1.enabled = false;
        }
        if (mouse2.enabled && paws.enabled)
        {
            AudioManager.Instance.PlayThumpSound();
            AudioManager.Instance.PlaySqueakSound();
            AddScore(1);
            mouse2.enabled = false;
        }
        if (mouse3.enabled && pawd.enabled)
        {
            AudioManager.Instance.PlayThumpSound();
            AudioManager.Instance.PlaySqueakSound();
            AddScore(1);
            mouse3.enabled = false;
        }
        if (mouse4.enabled && pawf.enabled)
        {
            AudioManager.Instance.PlayThumpSound();
            AudioManager.Instance.PlaySqueakSound();
            AddScore(1);
            mouse4.enabled = false;
        }
    }

    protected override void GameInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            pawa.enabled = true;
            paws.enabled = false;
            pawd.enabled = false;
            pawf.enabled = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            pawa.enabled = false;
            paws.enabled = true;
            pawd.enabled = false;
            pawf.enabled = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            pawa.enabled = false;
            paws.enabled = false;
            pawd.enabled = true;
            pawf.enabled = false;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            pawa.enabled = false;
            paws.enabled = false;
            pawd.enabled = false;
            pawf.enabled = true;
        }
    }

    private IEnumerator MousePop(Image mouseNum)
    {
        // Toggle visibility
        mouseNum.enabled = !mouseNum.enabled;

        float waitTime = Random.Range(minTime, maxTime);
        yield return new WaitForSecondsRealtime(waitTime);
        StartCoroutine(MousePop(mouseNum));
    }

    void AddScore(int scoreToAdd)
    {
        if (gameOver)
            return;
        if (!(curScore == 0 && scoreToAdd < 0))
        {
            curScore += scoreToAdd;
        }
    }

    void ResetGame()
    {
        gameOver = false;
        curScore = 0;
        linearTimer.StartTimer(gameTime);
        // subscribe to timer event
        linearTimer.OnTimerEnd += HandleGameOver;
    }

    void HandleGameOver()
    {
        gameOver = true;
        GameOver();

        // Stop minigame music and resume background music
        AudioManager.Instance.EndMinigame();
    }
}
