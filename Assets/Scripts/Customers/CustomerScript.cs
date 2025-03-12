using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

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

    public int hourStayed = 0; // how long a customer has been in the cafe
    [SerializeField] private Vector3 buffer; // Positional buffer for destination checking

    public int chair;

    public void SetAttributes(List<Attribute> attr)
    {
        foreach (var attribute in attr)
        {
            AddAttribute(attribute);
        }

        Debug.Log($"Manager selection: {string.Join(", ", attr)}");
        Debug.Log($"Customer selection: {string.Join(", ", attributes)}");
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

    public void Exit()
    {
        Debug.Log("Despawning Customer");
        gameObject.SetActive(false);
        attributes.Clear();
    }
}
