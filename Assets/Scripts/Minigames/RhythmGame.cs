using System;
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
    private float barBottom;
    private float barTop;
    private float startPos;
    private float endPos;

    [Header("Game Variables")]
    [SerializeField] private float fallSpeed;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    [SerializeField] private LinearTimer linearTimer;

    public override void StartGame()
    {
        base.StartGame();

        beatSpace = beat1.GetComponent<RectTransform>().rect.height / 2;
        barSpace = bar.GetComponent<RectTransform>().rect.height / 2;

        startPos = gameCanvas.GetComponent<RectTransform>().rect.height / 2 + beatSpace;
        endPos = -startPos;

        Debug.Log(beatSpace + " " + barSpace + " " + startPos + " ");

        ResetBeat(beat1);
        ResetBeat(beat2);
        ResetBeat(beat3);
        ResetBeat(beat4);

        barPos = bar.rectTransform.anchoredPosition.y;
        buffer = barSpace / 3;
        barBottom = barPos - barSpace - buffer;
        barTop = barPos + barSpace + buffer;

        linearTimer = FindObjectOfType<LinearTimer>();
        linearTimer.StartTimer(gameDuration);

        // subscribes a function to timer event
        linearTimer.OnTimerEnd += GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        GameInput();

        BeatDrop(beat1);
        BeatDrop(beat2);
        BeatDrop(beat3);
        BeatDrop(beat4);

        if (Time.realtimeSinceStartup - startTime > gameDuration)
        {
            GameOver();
        }
    }

    protected override void GameInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckBeat(beat1.rectTransform.anchoredPosition.y, beat1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CheckBeat(beat2.rectTransform.anchoredPosition.y, beat2);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CheckBeat(beat3.rectTransform.anchoredPosition.y, beat3);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CheckBeat(beat4.rectTransform.anchoredPosition.y, beat4);
        }
    }

    private void CheckBeat(float pos, Image beat)
    {
        float beatBottom = pos - beatSpace;
        float beatTop = pos + beatSpace;

        if ((beatTop <= barTop // lower than the top
        && beatTop >= barBottom) // but above the bottom
        || (beatBottom <= barTop
        && beatBottom >= barBottom))
        {
            // Beat is within bar
            ResetBeat(beat);
            curScore += Math.Abs(pos - barPos) / barSpace;
            Debug.Log("Beat " + (pos - barPos) / barSpace + " curScore " + curScore);
        }
    }

    private IEnumerator RandomBeat(Image beat)
    {
        float waitTime = UnityEngine.Random.Range(minTime, maxTime);
        yield return new WaitForSecondsRealtime(waitTime);

        beat.rectTransform.anchoredPosition = new Vector3(beat.rectTransform.anchoredPosition.x, startPos);
        beat.enabled = true;
    }

    private void BeatDrop(Image beat)
    {
        if (beat.enabled)
        {
            beat.transform.position += Vector3.down * fallSpeed * Time.unscaledDeltaTime;
            if (beat.rectTransform.anchoredPosition.y < endPos)
            {
                ResetBeat(beat);
            }
        }
    }

    private void ResetBeat(Image beat)
    {
        beat.enabled = false;
        beat.rectTransform.anchoredPosition = new Vector3(beat.rectTransform.anchoredPosition.x, startPos);
        StartCoroutine(RandomBeat(beat));
    }


}


