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

    // End Game Conditions
    private int daysPassed = 0;
    [SerializeField] GameObject cat1;
    [SerializeField] GameObject cat2;

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        if (!eventSystem)
        {
            Debug.Log("Did not find an Event System in this scene.", this);
            return;
        }

        if (PlayerPrefs.HasKey("NightOrDay"))
        {
            SaveScript.nightOrDayString = PlayerPrefs.GetString("NightOrDay");
            if (SaveScript.nightOrDayString == "night")
            {
                nightOrDay = NightOrDay.NIGHT;
                nightTimeMode.SetActive(true); // start at nighttime
                nightToDayButton.SetActive(true);
                hour = nightStartHour;
                clockSuffix = "pm";
            }
            else if (SaveScript.nightOrDayString == "day")
            {
                AudioManager.Instance.SwitchDayNight(true);
                nightOrDay = NightOrDay.DAY;
                nightTimeMode.SetActive(false); // start at nighttime
                nightToDayButton.SetActive(false);
                hour = dayStartHour;
                clockSuffix = "am";
            }
        }
        else
        {
            nightTimeMode.SetActive(true); // start at nighttime
            nightToDayButton.SetActive(true);
            hour = nightStartHour;
            SaveScript.SaveNightOrDay("night");
        }

        if (PlayerPrefs.HasKey("DaysPassed"))
        {
            SaveScript.daysPassed = PlayerPrefs.GetInt("DaysPassed");
            daysPassed = SaveScript.daysPassed;
        }

        if (PlayerPrefs.HasKey("Hour"))
        {
            SaveScript.hour = PlayerPrefs.GetInt("Hour");
            hour = SaveScript.hour;
        }

        if (PlayerPrefs.HasKey("DisplayHour"))
        {
            SaveScript.displayHour = PlayerPrefs.GetInt("DisplayHour");
            displayHour = SaveScript.displayHour;
        }

        if (PlayerPrefs.HasKey("Minute"))
        {
            SaveScript.minute = PlayerPrefs.GetInt("Minute");
            minute = SaveScript.minute;
        }

        if (PlayerPrefs.HasKey("Cat1Active"))
        {
            Debug.LogWarning("cat1Active is: " + SaveScript.cat1Active);
            if (SaveScript.cat1Active == 0)
            {
                Debug.LogWarning("cat1 set inactive");
                cat1.SetActive(false);
            }
        }

        if (PlayerPrefs.HasKey("Cat2Active"))
        {
            Debug.LogWarning("cat2Active is: " + SaveScript.cat2Active);
            if (SaveScript.cat2Active == 0)
            {
                Debug.LogWarning("cat2 set inactive");
                cat2.SetActive(false);
            }
        }

        pauseMenu.SetActive(false);
        displayHour = hour;
        if (displayHour >= 13)
        {
            displayHour -= 12;
        }
        if (minute == 0)
        {
            clockText.text = displayHour + ":00 " + clockSuffix;
        }
        else
        {
            clockText.text = displayHour + ":" + minute + " " + clockSuffix;
        }
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
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel")) && playerManager.playing == false)
        {
            Debug.Log("Pause");
            pauseButton.GetComponent<Button>().onClick.Invoke();
        }

        if (!cat1.activeSelf && !PlayerPrefs.HasKey("Cat1Active"))
        {
            SaveScript.SaveCat1(0);
        }

        if (!cat2.activeSelf && !PlayerPrefs.HasKey("Cat2Active"))
        {
            SaveScript.SaveCat2(0);
        }

        if (!cat1.activeSelf && !cat2.activeSelf)
        {
            SaveScript.DeleteSaves();
            SceneManager.LoadScene(5); // navigate to good end screen
        }

        if (daysPassed >= 2)
        {
            SaveScript.DeleteSaves();
            SceneManager.LoadScene(4); // navigates to the game over screen
        }
    }

    // MARK: Clock Functions
    private IEnumerator IncrementClock()
    {
        while (hour < (nightOrDay == NightOrDay.NIGHT ? nightEndHour : dayEndHour))
        {
            yield return new WaitForSeconds(secondsBeforeIncrement);

            minute += minutesIncrement;
            SaveScript.SaveMinute(minute);

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
            SaveScript.SaveHour(hour);
            SaveScript.SaveDisplayHour(displayHour);

            if (hour == 12) // switch to AM or PM
            {
                clockSuffix = nightOrDay == NightOrDay.NIGHT ? "am" : "pm";
            }
            if (displayHour >= 13)
            {
                displayHour -= 12;
                SaveScript.SaveDisplayHour(displayHour);
            }
            minute = minute - 60;
            SaveScript.SaveMinute(minute);

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
        daysPassed++;
        SaveScript.SaveDaysPassed(daysPassed);
        yield return new WaitForSeconds(transitionTime); // waits for the customers to leave
        nightOrDay = NightOrDay.NIGHT;
        SaveScript.SaveNightOrDay("night");

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
        Debug.Log("Switch to day");
        nightOrDay = NightOrDay.DAY;
        SaveScript.SaveNightOrDay("day");

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
        SaveScript.SaveMinute(minute);
        UpdateClock();
    }

    public void Restart()
    {
        AudioManager.Instance.SwitchDayNight(false);
        SaveScript.DeleteSaves();
        SceneManager.LoadScene(0); // loads the start menu
    }
}