using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

// clock code taken from https://github.com/kvanarsd/Bountiful-Game-Jam/blob/main/Assets/Scripts/GlobalManager.cs
// Credit: Katrina Vanarsdale (from a Game Jam Project)

// tenary statement snippet taken from https://discussions.unity.com/t/how-to-typecast-boolean-to-float-and-viceversa/420059/10
public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private CustomerManager customerManager; // to handle customer spawning
    [SerializeField] private DialogManager dialogManager; // to check on dialog status

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private GameObject pauseMenu;

    // Booleans
    private bool isPauseMenuOn = false;
    private bool isMinigameOn = false;
    private bool isGameOver = false;

    // Clock
    private int hour;
    private int displayHour;
    private int startHour = 8;
    private int endHour = 17;
    private int minute = 0;
    private int minutesIncrement = 10;
    [SerializeField] private float secondsBeforeIncrement = 5f;
    private string clockSuffix = "am";

    private void Start()
    {
        hour = startHour;
        displayHour = hour;
        clockText.text = "8:00 am";
        StartCoroutine(IncrementClock());

    }

    private void Update()
    {
        // // pause game when in dialog mode
        // if (dialogManager.IsPlaying && !isPauseMenuOn)
        // {
        //     Time.timeScale = 0;
        // }
        // if (!dialogManager.IsPlaying)
        // {
        //     Time.timeScale = 1;
        // }

    }

    private IEnumerator IncrementClock()
    {
        while (hour < endHour)
        {
            yield return new WaitForSeconds(secondsBeforeIncrement);

            minute += minutesIncrement;

            UpdateClock();
        }
        customerManager.ClosedCustomersLeave();
        SwitchToDay();
    }

    private void UpdateClock()
    {
        // end of hour
        if (minute >= 60)
        {
            // Check how long customer has been here 
            customerManager.CustomerHours();
            hour++;
            displayHour++;

            if (hour == 12) // switch to PM
            {
                clockSuffix = "pm";
            }

            if (displayHour == 13)
            {
                displayHour = 1;
            }

            minute = minute - 60;
            StartCoroutine(customerManager.CustomerWave());
        }

        if (minute == 0)
        {
            clockText.text = displayHour + ":00 " + clockSuffix;
        }
        else
        {
            clockText.text = displayHour + ":" + minute + " " + clockSuffix;
        }
    }

    private void ResetClock()
    {
        hour = startHour;
        displayHour = hour;
        minute = 0;
        clockSuffix = "am";
    }

    private void SwitchToNight()
    {

    }

    public void SwitchToDay()
    {
        Debug.Log("switch to day");
        ResetClock();
        StartCoroutine(IncrementClock());
    }

    public void PauseButton()
    {
        pauseMenu.SetActive(!isPauseMenuOn);
        isPauseMenuOn = !isPauseMenuOn;
        Time.timeScale = isPauseMenuOn ? 0f : 1f;
    }
}