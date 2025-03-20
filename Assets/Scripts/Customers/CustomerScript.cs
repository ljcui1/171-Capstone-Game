using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

// Saving serialization referenced from https://youtu.be/47QIUHDEaSY?si=M2AFwp2JimxTKGVj
[System.Serializable]
public class CustomerValues
{
    public int chair;
    public int hourStayed = 0;

    public CustomerValues(CustomerScript customer)
    {
        chair = customer.chair;
        hourStayed = customer.hourStayed;
    }
}

public class CustomerScript : BaseNPC
{
    // Customer-specific FSM-like flags
    public bool walkin = true;
    public bool sit = false;
    public bool talk = false;
    public bool bond = false;
    public bool reject = false;
    public bool accept = false;
    public bool walkout = false;

    public int hourStayed = 0;

    public int chair;

    public void SetAttributes(List<Attribute> attr)
    {
        attributes.Clear();
        foreach (var attribute in attr)
        {
            AddAttribute(attribute);
        }

        //Debug.Log($"Manager selection: {string.Join(", ", attr)}");
        //Debug.Log($"Customer selection: {string.Join(", ", attributes)}");
    }

    public void Exit()
    {
        //Debug.Log("Despawning Customer");
        attributes.Clear();
        Destroy(gameObject);
    }
}
