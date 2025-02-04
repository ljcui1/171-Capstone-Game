using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BaseNPC : MonoBehaviour
{


    public List<AttributePair> attributes = new List<AttributePair>();
    public AIDestinationSetter aiDestinationSetter;
    private AIPath aiPath;

    public SpriteRenderer mainSprite;
    public Collider2D playerInteract;


    protected virtual void Awake()
    {
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();

        if (aiDestinationSetter == null)
            Debug.LogError($"{name} is missing the AIDestinationSetter component!");
        if (aiPath == null)
            Debug.LogError($"{name} is missing the AIPath component!");
    }

    private void Start()
    {
        // Example of setting attributes
        SetAttribute(Attribute.Talkative, true);
        SetAttribute(Attribute.Foodie, false);
    }

    protected virtual void SetAttribute(Attribute attribute, bool isActive)
    {
        // Add or update the attribute in the dictionary
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].attribute == attribute)
            {
                attributes[i] = new AttributePair { attribute = attribute, isActive = isActive };
                return;
            }
        }
        attributes.Add(new AttributePair { attribute = attribute, isActive = isActive });
    }

    protected virtual bool IsAttributeActive(Attribute attribute)
    {
        // Check if the attribute is in the dictionary and active
        foreach (var attr in attributes)
        {
            if (attr.attribute == attribute)
            {
                return attr.isActive;
            }
        }
        return false;
    }

    public void SetDestination(GameObject target)
    {
        aiDestinationSetter.target = target.transform;
    }
}


public enum Attribute
{
    Talkative,
    Foodie,
    Active
}

[System.Serializable]
public struct AttributePair
{
    public Attribute attribute;
    public bool isActive;
}
