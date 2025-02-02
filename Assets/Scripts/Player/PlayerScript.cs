using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool inRange = false;
    public Collider2D talkTo;
    public SpriteRenderer npcSprite;
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
        if (other.tag == "Customer" || other.tag == "Cat")
        {
            inRange = true;
            talkTo = other;
            npcSprite = other.gameObject.GetComponent<SpriteRenderer>();
            npcTarget = other.gameObject.GetComponent<AIDestinationSetter>();
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
