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
    [SerializeField] private float minFallSpeed;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    private List<float> beatFallSpeed = new List<float>(new float[4]);

    public override void StartGame()
    {
        base.StartGame();
        // Start minigame music (Rhythm_Bed is Minigame #3)
        AudioManager.Instance.StartMinigame(3);
        beatSpace = beat1.GetComponent<RectTransform>().rect.height / 2;
        barSpace = bar.GetComponent<RectTransform>().rect.height / 2;

        startPos = gameCanvas.GetComponent<RectTransform>().rect.height / 2 + beatSpace;
        endPos = -startPos;

        Debug.Log(beatSpace + " " + barSpace + " " + startPos + " ");

        ResetBeat(beat1, 0);
        ResetBeat(beat2, 1);
        ResetBeat(beat3, 2);
        ResetBeat(beat4, 3);

        barPos = bar.rectTransform.anchoredPosition.y;
        buffer = barSpace / 3;
        barBottom = barPos - barSpace - buffer;
        barTop = barPos + barSpace + buffer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorial.activeSelf)
        {
            GameInput();

            BeatDrop(beat1, 0);
            BeatDrop(beat2, 1);
            BeatDrop(beat3, 2);
            BeatDrop(beat4, 3);

            /*if (Time.realtimeSinceStartup - startTime > gameDuration)
            {
                GameOver();
            }*/
        }
    }

    protected override void GameInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckBeat(beat1.rectTransform.anchoredPosition.y, beat1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CheckBeat(beat2.rectTransform.anchoredPosition.y, beat2, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CheckBeat(beat3.rectTransform.anchoredPosition.y, beat3, 2);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CheckBeat(beat4.rectTransform.anchoredPosition.y, beat4, 3);
        }
    }

    private void CheckBeat(float pos, Image beat, int beatIndex)
    {
        float beatBottom = pos - beatSpace;
        float beatTop = pos + beatSpace;

        if ((beatTop <= barTop // lower than the top
        && beatTop >= barBottom) // but above the bottom
        || (beatBottom <= barTop
        && beatBottom >= barBottom))
        {
            AudioManager.Instance.PlayMusicalMeow();
            // Beat is within bar
            ResetBeat(beat, beatIndex);
            AddScore((int)(Math.Abs(pos - barPos) / barSpace * 10));
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

    private void BeatDrop(Image beat, int beatIndex)
    {
        if (beat.enabled)
        {
            beat.transform.position += Vector3.down * beatFallSpeed[beatIndex] * Time.unscaledDeltaTime;
            if (beat.rectTransform.anchoredPosition.y < endPos)
            {
                ResetBeat(beat, beatIndex);
            }
        }
    }

    private void ResetBeat(Image beat, int beatIndex)
    {
        beat.enabled = false;
        beat.rectTransform.anchoredPosition = new Vector3(beat.rectTransform.anchoredPosition.x, startPos);
        float fallSpeed = UnityEngine.Random.Range(minFallSpeed, maxFallSpeed);
        beatFallSpeed[beatIndex] = fallSpeed;
        StartCoroutine(RandomBeat(beat));
    }


}


