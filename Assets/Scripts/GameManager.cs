using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject pauseButton;

    [Header("Day/Night Transition")]
    [SerializeField] private GameObject nightTimeMode;
    [SerializeField] private GameObject nightTransitionMsg;
    [SerializeField] private GameObject nightToDayButton;
    [SerializeField] private GameObject dayTimeMode;
    [SerializeField] private GameObject dayTransitionMsg;
    [SerializeField] private float msgTime; // time that the transition message stays on screen
    [SerializeField] private float transitionTime;
    public enum NightOrDay { NIGHT, DAY };
    public NightOrDay nightOrDay = NightOrDay.NIGHT;

    // Booleans
    private bool isPauseMenuOn = false;

    // Clock
    private int hour;
    private int displayHour;
    private int nightStartHour = 10;
    private int nightEndHour = 18;
    private int dayStartHour = 8;
    private int dayEndHour = 17;
    private int minute = 0;
    private int minutesIncrement = 10;
    [SerializeField] private float secondsBeforeIncrement = 5f;
    private string clockSuffix = "pm";

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        if (!eventSystem)
        {
            Debug.Log("Did not find an Event System in this scene.", this);
            return;
        }

        pauseMenu.SetActive(false);
        nightTimeMode.SetActive(true); // start at nighttime
        nightToDayButton.SetActive(true);
        hour = nightStartHour;
        displayHour = hour;
        clockText.text = displayHour + ":00 " + clockSuffix; // start time should be "10:00pm"
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
        if (Input.GetKeyDown(KeyCode.Escape) && playerManager.playing == false)
        {
            Debug.Log("Pause");
            pauseButton.GetComponent<Button>().onClick.Invoke();
        }
    }

    // MARK: Clock Functions
    private IEnumerator IncrementClock()
    {
        while (hour < (nightOrDay == NightOrDay.NIGHT ? nightEndHour : dayEndHour))
        {
            yield return new WaitForSeconds(secondsBeforeIncrement);

            minute += minutesIncrement;

            UpdateClock();
        }
        if (nightOrDay == NightOrDay.DAY)
        {
            customerManager.ClosedCustomersLeave();
            StartCoroutine(SwitchToNight());
        }
        else
        {
            StartCoroutine(SwitchToDay());
        }
    }

    private void UpdateClock()
    {
        // end of hour
        if (minute >= 60)
        {
            // clock UI management
            hour++;
            displayHour++;
            if (hour == 12) // switch to AM or PM
            {
                clockSuffix = nightOrDay == NightOrDay.NIGHT ? "am" : "pm";
            }
            if (displayHour == 13)
            {
                displayHour = 1;
            }
            minute = minute - 60;

            // customer management
            if (nightOrDay == NightOrDay.DAY)
            {
                customerManager.CustomerHours(); // Check how long customer has been here 
                StartCoroutine(customerManager.CustomerWave());
            }
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
        hour = nightOrDay == NightOrDay.NIGHT ? nightStartHour : dayStartHour;
        displayHour = hour;
        minute = 0;
        clockSuffix = nightOrDay == NightOrDay.NIGHT ? "pm" : "am";
    }

    // MARK: Day/Night
    private IEnumerator SwitchToNight()
    {
        yield return new WaitForSeconds(transitionTime); // waits for the customers to leave
        nightOrDay = NightOrDay.NIGHT;

        // UI
        nightTimeMode.SetActive(true);
        nightTransitionMsg.SetActive(true);
        ResetClock();
        clockText.text = displayHour + ":00 " + clockSuffix;
        AudioManager.Instance.SwitchDayNight(false);

        // Transition MSG UI
        yield return new WaitForSeconds(msgTime);
        nightTransitionMsg.SetActive(false);
        nightToDayButton.SetActive(true);
        StartCoroutine(IncrementClock());
    }

    private IEnumerator SwitchToDay()
    {
        yield return new WaitForSeconds(1f);
        nightOrDay = NightOrDay.DAY;

        // UI
        nightToDayButton.SetActive(false);
        nightTimeMode.SetActive(false);
        dayTransitionMsg.SetActive(true);
        ResetClock();
        clockText.text = displayHour + ":00 " + clockSuffix;
        AudioManager.Instance.SwitchDayNight(true);

        // Transition MSG UI
        yield return new WaitForSeconds(msgTime);
        dayTransitionMsg.SetActive(false);
        StartCoroutine(IncrementClock());
    }

    // Helper function for DialogScript.cs
    public TimeOfDay CurrentTime()
    {
        return nightTimeMode.activeSelf ? TimeOfDay.Night : TimeOfDay.Day;
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
        StopAllCoroutines();
        StartCoroutine(SwitchToDay());
    }

    public void MinigamesFinished()
    {
        minute += 30;
        UpdateClock();
    }

    public void Restart()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Application.LoadLevel(0);
        SceneManager.LoadScene(0); // loads the start menu
    }
}