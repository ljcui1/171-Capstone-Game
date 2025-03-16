using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public static class SaveScript
{
    public static string nightOrDayString;
    public static int daysPassed;
    public static int cat1Active;
    public static int cat2Active;
    public static float customerWeights;
    public static int customerNum;

    public static void SaveNightOrDay(string time)
    {
        nightOrDayString = time;
        PlayerPrefs.SetString("NightOrDay", nightOrDayString);
        Debug.Log("time has been set to: " + time);
    }

    public static void SaveDaysPassed(int days)
    {
        daysPassed = days;
        PlayerPrefs.SetInt("DaysPassed", daysPassed);
        Debug.Log("daysPassed has been set to: " + daysPassed);
    }

    public static void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
