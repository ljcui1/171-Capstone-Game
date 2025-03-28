using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private float minTime;

    [SerializeField]
    private float maxTime;

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
        if (!tutorial.activeSelf)
        {
            GameInput();

            /*if (Time.realtimeSinceStartup - startTime > gameDuration)
            {
                GameOver();
            }*/

            CheckCatching(mouse1, pawa);
            CheckCatching(mouse2, paws);
            CheckCatching(mouse3, pawd);
            CheckCatching(mouse4, pawf);
        }
    }

    void CheckCatching(Image mouse, Image paw)
    {
        if (mouse.enabled && paw.enabled)
        {
            AudioManager.Instance.PlayThumpSound();
            AudioManager.Instance.PlaySqueakSound();
            AddScore(1);
            mouse.enabled = false;
            paw.enabled = false;
        }
    }

    protected override void GameInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButton("ButtonA"))
        {
            pawa.enabled = true;
            paws.enabled = false;
            pawd.enabled = false;
            pawf.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetButton("ButtonB"))
        {
            pawa.enabled = false;
            paws.enabled = true;
            pawd.enabled = false;
            pawf.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetButton("ButtonX"))
        {
            pawa.enabled = false;
            paws.enabled = false;
            pawd.enabled = true;
            pawf.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F) || Input.GetButton("ButtonY"))
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
