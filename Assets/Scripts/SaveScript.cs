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

    public static void SaveWeights()
    {
        foreach (
            KeyValuePair<Attribute, float> keyValue in CustomerManager.instance.attributeWeights
        )
        {
            PlayerPrefs.SetFloat(nameof(keyValue.Key), keyValue.Value);
        }
    }

    public static void LoadWeights()
    {
        foreach (
            KeyValuePair<Attribute, float> keyValue in CustomerManager.instance.attributeWeights
        )
        {
            CustomerManager.instance.attributeWeights[keyValue.Key] = PlayerPrefs.GetFloat(nameof(keyValue.Key));
        }
    }

    // saving and loading customers referenced from https://youtu.be/47QIUHDEaSY?si=4RsCfqj4ljE8AhyK
    public static void SaveCustomers()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + customerSub;
        string countPath = Application.persistentDataPath + customerCountSub;

        FileStream countStream = new FileStream(countPath, FileMode.Create);

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

        if (File.Exists(countPath))
        {
            FileStream countStream = new FileStream(countPath, FileMode.Open);

            custCount = (int)formatter.Deserialize(countStream);
            countStream.Close();
        }
        else
        {
            Debug.LogError("Path not found in " + countPath);
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

                CustomerManager.instance.customerPool = new List<CustomerScript>();
                CustomerManager.instance.customerPool.Add(script);
                script.chair = data.chair;
                script.hourStayed = data.hourStayed;
                script.sit = true;
            }
            else
            {
                Debug.LogError("Path not found in " + (path + i));
            }
        }
    }

    public static void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
