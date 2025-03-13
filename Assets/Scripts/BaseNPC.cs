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
    // public Collider2D playerInteract;
    [SerializeField] private Vector3 buffer;

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
        // // Example of setting attributes
        // SetAttribute(Attribute.Talkative, true);
        // SetAttribute(Attribute.Foodie, false);
    }

    public void SetAttribute(Attribute attribute, bool isActive)
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
        // attributes.Add(new AttributePair { attribute = attribute, isActive = isActive });
    }

    public void AddAttribute(Attribute attribute)
    {
        // Add the attribute to the dictionary
        if (!attributes.Exists(x => x.attribute == attribute))
        {
            attributes.Add(new AttributePair { attribute = attribute, isActive = false });
        }
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

    public bool AtDestination()
    {
        if (aiDestinationSetter.target != null)
        {
            Vector3 targetPosition = aiDestinationSetter.target.position;
            Vector3 currentPosition = transform.position;

            // Check if within buffer bounds
            if (Mathf.Abs(targetPosition.x - currentPosition.x) <= buffer.x &&
                Mathf.Abs(targetPosition.y - currentPosition.y) <= buffer.y)
            {
                aiDestinationSetter.target = null; // Clear the target once arrived
                return true;
            }
        }

        return false;
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
