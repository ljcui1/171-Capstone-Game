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
    public bool walkout = true;     // set by GameManager (based on ingame time) unless "accept" 

    // Attributes
    private List<string> attributes;

    // pathing destination
    public AIDestinationSetter destination;
    [SerializeField] private Vector3 buffer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAttributes(List<string> attr)
    {
        attributes = attr;
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

    private void BondDecision(GameObject cat)
    {
        //if (cat.GetComponent<CatScript>.att)
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
