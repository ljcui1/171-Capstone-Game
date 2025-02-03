using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool inRange = false;
    public Collider2D talkTo;
    public SpriteRenderer catSprite;
    public SpriteRenderer custSprite;
    public AIDestinationSetter npcTarget;
    public GameObject npc;
    public bool startPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Cat")
        {
            inRange = true;
            talkTo = other;
            catSprite = other.gameObject.GetComponent<SpriteRenderer>();
            npcTarget = other.gameObject.GetComponent<AIDestinationSetter>();
        }
        else if (other.tag == "Customer")
        {
            inRange = true;
            talkTo = other;
            custSprite = other.gameObject.GetComponent<SpriteRenderer>();
            npc = other.gameObject;
        }
        else if (other.tag == "mouse")
        {
            startPlay = true;
            Debug.Log("q to start");
        }
        else
        {
            inRange = false;
            talkTo = null;
        }
    }
}
