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
    public static int cat1Active = 1;
    public static int cat2Active = 1;
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
                script.chair = data.chair;
                script.hourStayed = data.hourStayed;
                script.sit = true;
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
