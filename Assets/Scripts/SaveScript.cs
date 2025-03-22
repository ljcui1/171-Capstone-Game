using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using Unity.VisualScripting;
//using System.Numerics;

public static class SaveScript
{
    public static string nightOrDayString;
    public static int daysPassed;
    public static int hour;
    public static int displayHour;
    public static int minute;
    public static int cat1Active;
    public static int cat2Active;
    public static int customerNum;

    const string customerSub = "/cust";
    const string customerCountSub = "/cust.count";

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

    public static void SaveHour(int savedHour)
    {
        hour = savedHour;
        PlayerPrefs.SetInt("Hour", hour);
        Debug.Log("hour has been set to: " + hour);
    }

    public static void SaveDisplayHour(int savedDisplayHour)
    {
        displayHour = savedDisplayHour;
        PlayerPrefs.SetInt("DisplayHour", displayHour);
        Debug.Log("displayHour has been set to: " + displayHour);
    }

    public static void SaveMinute(int savedMinute)
    {
        minute = savedMinute;
        PlayerPrefs.SetInt("Minute", minute);
        Debug.Log("minute has been set to: " + minute);
    }

    public static void SaveCat1(int cat1)
    {
        cat1Active = cat1;
        PlayerPrefs.SetInt("Cat1Active", cat1Active);
        Debug.Log("cat1Active has been set to: " + cat1Active);
    }

    public static void SaveCat2(int cat2)
    {
        cat2Active = cat2;
        PlayerPrefs.SetInt("Cat2Active", cat2Active);
        Debug.Log("cat2Active has been set to: " + cat2Active);
    }

    public static void SaveCatData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string catPath = Application.persistentDataPath + "/cat";
        string catCountPath = Application.persistentDataPath + "/cat.count";

        // Save the number of cats.
        FileStream countStream = new FileStream(catCountPath, FileMode.Create);
        formatter.Serialize(countStream, CatManager.instance.cats.Count);
        countStream.Close();

        // Save each cat's data.
        for (int i = 0; i < CatManager.instance.cats.Count; i++)
        {
            GameObject catObj = CatManager.instance.cats[i];
            CatScript catScript = catObj.GetComponent<CatScript>();
            if (catScript != null)
            {
                Debug.Log("Saving cat data for " + i);
                FileStream stream = new FileStream(catPath + i, FileMode.Create);
                CatValues data = new CatValues(catScript);
                formatter.Serialize(stream, data);
                stream.Close();
            }
        }
    }

    public static void LoadCatData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string catPath = Application.persistentDataPath + "/cat";
        string catCountPath = Application.persistentDataPath + "/cat.count";
        int savedCatCount = 0;

        if (File.Exists(catCountPath))
        {
            FileStream countStream = new FileStream(catCountPath, FileMode.Open);
            savedCatCount = (int)formatter.Deserialize(countStream);
            countStream.Close();
        }
        else
        {
            Debug.LogWarning("No saved cat count found.");
            return;
        }

        // Assume CatManager has already spawned its cats.
        for (int i = 0; i < savedCatCount; i++)
        {
            string filePath = catPath + i;
            if (File.Exists(filePath))
            {
                FileStream stream = new FileStream(filePath, FileMode.Open);
                CatValues savedCat = formatter.Deserialize(stream) as CatValues;
                stream.Close();

                if (i < CatManager.instance.cats.Count)
                {
                    Debug.Log("Loading cat data for " + i);
                    CatScript catScript = CatManager.instance.cats[i].GetComponent<CatScript>();
                    // Update the cat's saved values. For example, updating its attributes:
                    Debug.Log(savedCat.attributes + savedCat.matched.ToString());
                    foreach (AttributePair attPair in savedCat.attributes)
                    {
                        Debug.Log(attPair.attribute.ToString() + attPair.isActive.ToString());
                    }

                    catScript.SetAttributePairs(savedCat.attributes);
                    catScript.matched = savedCat.matched;
                    DialogScript catDialog = CatManager.instance.cats[i].GetComponent<DialogScript>();
                    catDialog.SetEntryPlayed();
                }
                else
                {
                    Debug.LogWarning($"Saved cat data at index {i} but no corresponding cat was spawned.");
                }
            }
            else
            {
                Debug.LogWarning("Cat file not found: " + filePath);
            }
        }
    }




    public static void SaveCustSpawnData()
    {
        // save attribute weights
        foreach (
            KeyValuePair<Attribute, float> keyValue in CustomerManager.instance.attributeWeights
        )
        {
            PlayerPrefs.SetFloat(nameof(keyValue.Key), keyValue.Value);
        }

        // save chair occupied
        int occupancy;
        for (int i = 0; i < CustomerManager.instance.chairOccupied.Count; i++)
        {
            if (CustomerManager.instance.chairOccupied[i])
            {
                occupancy = 1;
            }
            else
            {
                occupancy = 0;
            }
            PlayerPrefs.SetInt("chair" + i, occupancy);
        }

        // save number of customers to spawn
        PlayerPrefs.SetInt("numCustomer", CustomerManager.instance.numCustomersToSpawn);
    }

    public static void LoadCustSpawnData()
    {
        // load attribute weights
        foreach (Attribute att in CustomerManager.instance.attributes)
        {
            CustomerManager.instance.attributeWeights[att] = PlayerPrefs.GetFloat(nameof(att), 0);
        }

        // load chair occupied
        bool occupancy;
        for (int i = 0; i < CustomerManager.instance.chairOccupied.Count; i++)
        {
            if (PlayerPrefs.GetInt("chair" + i, 0) == 1)
            {
                occupancy = true;
            }
            else
            {
                occupancy = false;
            }
            CustomerManager.instance.chairOccupied[i] = occupancy;
        }

        // load number of customers to spawn
        CustomerManager.instance.numCustomersToSpawn = PlayerPrefs.GetInt("numCustomer", 1);
    }

    // saving and loading customers referenced from https://youtu.be/47QIUHDEaSY?si=4RsCfqj4ljE8AhyK
    public static void SaveCustomers()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + customerSub;
        string countPath = Application.persistentDataPath + customerCountSub;

        Debug.Log("Before creation " + File.Exists(countPath));
        FileStream countStream = new FileStream(countPath, FileMode.Create);
        Debug.Log("After creation " + File.Exists(countPath));
        formatter.Serialize(countStream, CustomerManager.instance.customerPool.Count);
        countStream.Close();

        for (int i = 0; i < CustomerManager.instance.customerPool.Count; i++)
        {
            FileStream stream = new FileStream(path + i, FileMode.Create);
            CustomerValues data = new CustomerValues(CustomerManager.instance.customerPool[i]);

            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    public static void LoadCustomer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + customerSub;
        string countPath = Application.persistentDataPath + customerCountSub;
        int custCount = 0;

        Debug.Log("Loading does " + File.Exists(countPath));
        if (File.Exists(countPath))
        {
            Debug.Log("Found path");
            FileStream countStream = new FileStream(countPath, FileMode.Open);

            custCount = (int)formatter.Deserialize(countStream);
            countStream.Close();
        }
        else
        {
            return;
        }

        for (int i = 0; i < custCount; i++)
        {
            if (File.Exists(path + i))
            {
                FileStream stream = new FileStream(path + i, FileMode.Open);
                CustomerValues data = formatter.Deserialize(stream) as CustomerValues;

                stream.Close();

                Vector3 position = CustomerManager.instance.chairs[data.chair].transform.position;

                GameObject customer = Object.Instantiate(CustomerManager.instance.customerPrefab, position, Quaternion.identity, CustomerManager.instance.transform);
                CustomerScript script = customer.GetComponent<CustomerScript>();
                CustomerManager.instance.customerPool.Add(script);
                script.num = data.num;
                script.chair = data.chair;
                script.hourStayed = data.hourStayed;
                script.sit = true;
                script.SetAttributePairs(data.attributes);
            }
            else
            {
                Debug.Log("Path not found in " + (path + i));
            }
        }
    }

    public static void DeleteSaves()
    {
        Debug.Log("Deleting saves");
        PlayerPrefs.DeleteAll();
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        foreach (string filePath in filePaths)
            File.Delete(filePath);
    }
}
