using UnityEngine;
using UnityEngine.UI;
using System;

public class LinearTimer : MonoBehaviour
{
    public Slider timerSlider;
    private float timer;
    private bool isRunning;

    public event Action OnTimerEnd; // Event to notify when timer finishes

    public void StartTimer(float gameTime)
    {
        timer = gameTime;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            timer -= Time.unscaledDeltaTime;
            timerSlider.value = Mathf.Max(0, timer); // Prevent going below 0

            if (timer <= 0)
            {
                isRunning = false;
                OnTimerEnd?.Invoke(); // Notify subscribers that time is up
            }
        }
    }

    public float GetRemainingTime()
    {
        return timer;
    }

    public bool IsTimerRunning()
    {
        return isRunning;
    }
}
