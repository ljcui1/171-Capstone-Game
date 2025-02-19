using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGame : BaseMinigame
{
    [Header("Beat Images")]
    [SerializeField] private Image beat1;
    [SerializeField] private Image beat2;
    [SerializeField] private Image beat3;
    [SerializeField] private Image beat4;

    [Header("Beat Sounds")]
    [SerializeField] private AudioClip sound1;
    [SerializeField] private AudioClip sound2;
    [SerializeField] private AudioClip sound3;
    [SerializeField] private AudioClip sound4;

    [Header("Positions")]
    private float beatSpace;
    [SerializeField] private Image bar;
    private float barPos;
    private float barSpace;
    private float buffer;
    private Vector3 startPos;
    private Vector3 endPos;

    [Header("Game Variables")]
    [SerializeField] private float fallSpeed;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    private Vector3[] corners;

    public override void StartGame()
    {
        base.StartGame();

        corners = new Vector3[4];

        beatSpace = beat1.GetComponent<RectTransform>().rect.height / 2;
        barSpace = bar.GetComponent<RectTransform>().rect.height / 2;

        startPos = Vector3.down * (gameCanvas.GetComponent<RectTransform>().rect.height / 2 + beatSpace);
        endPos = -startPos;

        StartCoroutine(RandomBeat(gameDuration, beat1));
        StartCoroutine(RandomBeat(gameDuration, beat2));
        StartCoroutine(RandomBeat(gameDuration, beat3));
        StartCoroutine(RandomBeat(gameDuration, beat4));

        //barPos = barTransform.position.y;


    }

    // Update is called once per frame
    void Update()
    {
        GameInput();

        if (Time.realtimeSinceStartup - startTime > gameDuration)
        {
            GameOver();
        }
    }

    protected override void GameInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            CheckBeat(beat1.transform.position.y, beat1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            CheckBeat(beat2.transform.position.y, beat2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            CheckBeat(beat3.transform.position.y, beat3);
        }
        else if (Input.GetKey(KeyCode.F))
        {
            CheckBeat(beat4.transform.position.y, beat4);
        }
    }

    private RectTransform ObjectHeight(Image image)
    {
        RectTransform size = image.GetComponent<RectTransform>();
        return size;
    }

    private void CheckBeat(float pos, Image beat)
    {
        if (pos + beatSpace <= barPos + barSpace + buffer
        && pos - beatSpace >= barPos - barSpace - buffer)
        {
            // Beat is within bar
            beat.enabled = false;
            curScore += (pos - barPos) / barSpace;
            Debug.Log("Beat " + (pos - barPos) / barSpace + " curScore " + curScore);
        }
    }

    private IEnumerator RandomBeat(float seconds, Image beatNum)
    {
        float startTime = Time.realtimeSinceStartup;
        beatNum.transform.position += startPos;
        beatNum.enabled = true;

        float waitTime = Random.Range(minTime, maxTime);
        yield return new WaitForSecondsRealtime(waitTime);
        StartCoroutine(RandomBeat(seconds, beatNum));
    }

    private void BeatDrop(Image beatNum)
    {
        if (beatNum.enabled)
        {
            beatNum.transform.position += Vector3.down * fallSpeed * Time.unscaledDeltaTime;
            if (beatNum.transform.position.y > endPos.y)
            {
                beatNum.enabled = false;
            }
        }
    }
}


