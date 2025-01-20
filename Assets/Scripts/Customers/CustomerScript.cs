using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    // All customer states FSM
    public bool walk = true;
    public bool sit = false;
    public bool bond = false;
    public bool reject = false;
    public bool accept = false;

    // Attributes
    private List<string> attributes;

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
}
