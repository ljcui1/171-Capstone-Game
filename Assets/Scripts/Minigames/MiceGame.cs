using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiceGame : BaseMinigame
{

    //public Canvas mouseGame;

    [SerializeField]
    private Image pawa;

    [SerializeField]
    private Image paws;

    [SerializeField]
    private Image pawd;

    [SerializeField]
    private Image pawf;

    [SerializeField]
    private Image mouse1;

    [SerializeField]
    private Image mouse2;

    [SerializeField]
    private Image mouse3;

    [SerializeField]
    private Image mouse4;
    public override void StartGame()
    {
        base.StartGame();
        StartCoroutine(MousePop(30f, mouse1));
        StartCoroutine(MousePop(30f, mouse2));
        StartCoroutine(MousePop(30f, mouse3));
        StartCoroutine(MousePop(30f, mouse4));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Mice game is updating");
        GameInput();
    }



    public void GameInput()
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

    private IEnumerator MousePop(float seconds, Image mouseNum)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < seconds)
        {
            // Toggle visibility
            mouseNum.enabled = !mouseNum.enabled;

            float waitTime = Random.Range(1f, 3f);
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

}

