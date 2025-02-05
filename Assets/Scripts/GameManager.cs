using System.Collections;
using System.Collections.Generic;
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
    private int hour = 8;
    private int displayHour = 8;
    private int endHour = 17;
    private int minute = 0;
    private int minutesIncrement = 10;
    [SerializeField] private float secondsBeforeIncrement = 5f;
    private string clockSuffix = "am";

    private void Start()
    {
        clockText.text = "8:00 am";
        StartCoroutine(IncrementClock());

    }

    private void Update()
    {
        // if (isPauseMenuOn)
        // {
        //     Time.timeScale = 0;
        // }
        // else if (!isPauseMenuOn)
        // {
        //     Time.timeScale = 1;
        // }
        // // pause game when in dialog mode
        // if (!dialogManager.IsPlaying && !isPauseMenuOn && Time.timeScale == 0)
        // {

        // }

    }

    private IEnumerator IncrementClock()
    {
        yield return new WaitForSeconds(secondsBeforeIncrement);

        minute += minutesIncrement;

        UpdateClock();

        StartCoroutine(IncrementClock());
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

        if (hour >= endHour)
        {
            isGameOver = true;
            Debug.Log("Game reached 5:00PM");
            Time.timeScale = 0;
        }
    }

    public void PauseButton()
    {
        pauseMenu.SetActive(!isPauseMenuOn);
        isPauseMenuOn = !isPauseMenuOn;
        Time.timeScale = isPauseMenuOn ? 0f : 1f;
    }
}