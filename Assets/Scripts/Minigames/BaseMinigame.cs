using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMinigame : MonoBehaviour
{
    [SerializeField] private int maxScore;
    private int curScore = 0;
    [SerializeField] private Attribute attribute;

    private MinigameManager manager = MinigameManager.instance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GameOver()
    {
        manager.GameScore(curScore, maxScore, attribute);
        curScore = 0;
    }
    /*
        // function is referenced from https://discussions.unity.com/t/waitforseconds-while-time-scale-0/552937
        private IEnumerator WaitForRealSecond(float seconds)
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < seconds)
            {
                yield return null;
            }
        }
    */
}
