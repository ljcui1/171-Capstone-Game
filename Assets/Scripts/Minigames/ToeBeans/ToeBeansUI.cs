using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToeBeansUI : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI timerUI;
    private void Start()
    {
        transform.GetComponent<SetsUiElementToSelectOnInteraction>().FindEventSystem();
        transform.GetComponent<SetsUiElementToSelectOnInteraction>().JumpToElement();
    }
    public void UpdateScoreUI(float curScore)
    {
        scoreUI.SetText(curScore.ToString());
    }

    // public void UpdateTimerUI(int timeRemaining)
    // {
    //     timerUI.SetText(timeRemaining.ToString());
    // }
}
