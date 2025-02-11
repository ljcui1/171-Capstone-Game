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
    [SerializeField] private float beatSpace;
    [SerializeField] private Transform barTransform;
    private float barPos;
    [SerializeField] private float barSpace;
    [SerializeField] private float buffer;
    [SerializeField] private float startPos;

    [SerializeField] private Conductor conductor;

    [SerializeField] private float fallSpeed;
    private float distanceToHitBar;

    // Start is called before the first frame update
    void Start()
    {
        barPos = barTransform.position.y;
        distanceToHitBar = barPos - startPos;
    }

    // Update is called once per frame
    void Update()
    {
        GameInput();
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

    private void CheckBeat(float pos, Image beat)
    {
        if (pos + beatSpace <= barPos + barSpace + buffer
        && pos - beatSpace >= barPos - barSpace - buffer)
        {
            // Beat is within bar
            beat.enabled = false;
            curScore += (pos - barPos) / barSpace;
        }
    }

    // referenced from https://chatgpt.com/share/67abaa3a-83a0-800b-9188-598d8dfb3fd6
    private void Beats(Image beatNum)
    {
        Vector3 pos = beatNum.transform.position;
        pos.y = barPos;
        Debug.Log("Before " + beatNum.transform.position);
        beatNum.transform.position = pos;
        Debug.Log("after " + beatNum.transform.position);
        beatNum.enabled = true;

        float timeToReachBar = distanceToHitBar / fallSpeed;
        float spawnTime = timeToReachBar;

        // Ensure note spawns at correct time
        StartCoroutine(SpawnAtTime(spawnTime));
    }

    IEnumerator SpawnAtTime(float targetTime)
    {
        while (conductor.songPosition < targetTime)
        {
            yield return null;
        }

    }
}


