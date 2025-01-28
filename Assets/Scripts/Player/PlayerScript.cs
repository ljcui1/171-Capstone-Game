using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public LineRenderer lr;
    public bool inRange = false;
    public Collider2D talkTo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        lr.positionCount = 2;
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Customer" || other.tag == "Cat")
        {
            inRange = true;
            talkTo = other;
        }
        else
        {
            inRange = false;
            talkTo = null;
        }
    }
}
