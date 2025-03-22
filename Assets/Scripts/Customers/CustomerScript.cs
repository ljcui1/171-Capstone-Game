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
    public int num;
    public List<AttributePair> attributes; // New field

    public CustomerValues(CustomerScript customer)
    {
        chair = customer.chair;
        hourStayed = customer.hourStayed;
        num = customer.num;

        attributes = new List<AttributePair>(customer.attributes);
    }
}

public class CustomerScript : BaseNPC
{
    [Header("FSM")]
    // Customer-specific FSM-like flags
    public bool walkin = true;
    public bool sit = false;
    public bool talk = false;
    public bool bond = false;
    public bool reject = false;
    public bool accept = false;
    public bool walkout = false;

    [Header("Data")]
    public int hourStayed = 0;
    public int chair;

    [Header("Animations")]
    public Animator anim;
    public AIPath ai;
    public int num;

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

    public void SetAttributePairs(List<AttributePair> attr)
    {
        attributes.Clear();
        foreach (var attribute in attr)
        {
            attributes.Add(new AttributePair { attribute = attribute.attribute, isActive = attribute.isActive });
        }
    }

    public void Exit()
    {
        //Debug.Log("Despawning Customer");
        attributes.Clear();
        Destroy(gameObject);
    }
}
