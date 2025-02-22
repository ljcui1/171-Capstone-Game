using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiceGame : BaseMinigame
{
    private LinearTimer linearTimer;
    public bool gameOver = false;
    public float gametime = 20f;
    public int curScore;
    public int maxScore;
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
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    void Awake()
    {
        linearTimer = FindObjectOfType<LinearTimer>();
        linearTimer.StartTimer(gametime);

        linearTimer.OnTimerEnd += GameOver;
    }
    public override void StartGame()
    {
        base.StartGame();
        StartCoroutine(MousePop(mouse1));
        StartCoroutine(MousePop(mouse2));
        StartCoroutine(MousePop(mouse3));
        StartCoroutine(MousePop(mouse4));
    }

    // Update is called once per frame
    void Update()
    {
        GameInput();

        if (mouse1.enabled == true && pawa.enabled == true)
        {
            curScore += 1;
        }
        if (mouse2.enabled == true && paws.enabled == true)
        {
            curScore += 1;
        }
        if (mouse3.enabled == true && pawd.enabled == true)
        {
            curScore += 1;
        }
        if (mouse4.enabled == true && pawf.enabled == true)
        {
            curScore += 1;
        }
        /*if (Time.realtimeSinceStartup - startTime > gameDuration)
        {
            GameOver();
        }*/
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

}

