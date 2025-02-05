using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    // All customer states FSM
    public bool walkin = true;
    public bool sit = false;
    public bool talk = false;
    public bool bond = false;
    public bool reject = false;
    public bool accept = false;
    public bool walkout = false;

    public int hourStayed = 0; // how long a customer has been in the cafe

    [SerializeField] private Vector3 buffer; // Positional buffer for destination checking

    public void SetAttributes(List<GameManager.Attribute> attr)
    {
        activeAttributes = new(attr);
    }

    public void SetDestination(GameObject target)
    {
        destination.target = target.transform;
    }

    public bool AtDestination()
    {
        if (destination.target != null)
        {
            if (
                destination.target.position.x - buffer.x <= transform.position.x
                && transform.position.x <= destination.target.position.x + buffer.x
                && destination.target.position.y - buffer.y <= transform.position.y
                && transform.position.y <= destination.target.position.y + buffer.y
            )
            {
                destination.target = null;
                return true;
            }
        }

        return false;
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
