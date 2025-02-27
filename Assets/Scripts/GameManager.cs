using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// clock code taken from https://github.com/kvanarsd/Bountiful-Game-Jam/blob/main/Assets/Scripts/GlobalManager.cs
// Credit: Katrina Vanarsdale (from a Game Jam Project)

// tenary statement snippet taken from https://discussions.unity.com/t/how-to-typecast-boolean-to-float-and-viceversa/420059/10
public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private CustomerManager customerManager; // to handle customer spawning
    [SerializeField] private DialogManager dialogManager; // to check on dialog status
    [SerializeField] private PlayerManager playerManager;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private GameObject pauseMenu;

    [Header("Day/Night Transition")]
    [SerializeField] private GameObject nightTimeMode;
    [SerializeField] private GameObject nightTransitionMsg;
    [SerializeField] private GameObject dayTimeMode;
    [SerializeField] private GameObject dayTransitionMsg;
    [SerializeField] private float msgTime; // time that the transition message stays on screen
    [SerializeField] private float transitionTime;

    // Booleans
    private bool isPauseMenuOn = false;

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
        pauseMenu.SetActive(false);
        nightTimeMode.SetActive(false);
        hour = startHour;
        displayHour = hour;
        clockText.text = "8:00 am";
        StartCoroutine(IncrementClock());
    }

    private void Update()
    {
        // pause game when in dialog mode
        if (dialogManager.IsPlaying && !isPauseMenuOn && !playerManager.playing)
        {
            Time.timeScale = 0;
        }
        if (!dialogManager.IsPlaying && !isPauseMenuOn && !playerManager.playing)
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseButton();
        }
    }

    // MARK: Clock Functions
    private IEnumerator IncrementClock()
    {
        while (hour < endHour)
        {
            yield return new WaitForSeconds(secondsBeforeIncrement);

            minute += minutesIncrement;

            UpdateClock();
        }
        customerManager.ClosedCustomersLeave();
        StartCoroutine(SwitchToNight());
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
            //Debug.Log("CustomerWave");
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

    // MARK: Day/Night
    private IEnumerator SwitchToNight()
    {
        yield return new WaitForSeconds(transitionTime); // waits for the customers to leave
        clockText.text = "12:00 am"; // time is arbitrary; just set it to some feasible night time hour
        nightTimeMode.SetActive(true);
        nightTransitionMsg.SetActive(true);
        yield return new WaitForSeconds(msgTime);
        nightTransitionMsg.SetActive(false);
        AudioManager.Instance.SwitchDayNight(false);
    }
    public TimeOfDay CurrentTime()
    {
        return nightTimeMode.activeSelf ? TimeOfDay.Night : TimeOfDay.Day;
    }
    private IEnumerator SwitchToDay()
    {
        // Debug.Log("switch to day");
        nightTimeMode.SetActive(false);
        dayTransitionMsg.SetActive(true);
        ResetClock();
        clockText.text = "8:00 am";
        AudioManager.Instance.SwitchDayNight(true);
        yield return new WaitForSeconds(msgTime);
        dayTransitionMsg.SetActive(false);
        StartCoroutine(IncrementClock());
    }

    // MARK: Button Functions
    public void PauseButton()
    {
        pauseMenu.SetActive(!isPauseMenuOn);
        isPauseMenuOn = !isPauseMenuOn;
        Time.timeScale = isPauseMenuOn ? 0f : 1f;
    }

    public void StartNextDay()
    {
        StartCoroutine(SwitchToDay());
    }

    public void MinigamesFinished()
    {
        minute += 30;
        UpdateClock();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Application.LoadLevel(0);
    }
}