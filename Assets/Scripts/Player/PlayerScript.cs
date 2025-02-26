using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool inRange = false;
    public Collider2D talkTo;
    public Collider2D catCollide;
    public Collider2D custCollide;
    public Collider2D gameCollide;
    // public SpriteRenderer catSprite;
    // public SpriteRenderer custSprite;
    // public AIDestinationSetter npcTarget;
    // public GameObject npc;
    public BaseNPC cat;
    public BaseNPC customer;
    public GameObject npcTarget;
    public bool startPlay = false;

    public Animator anims;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anims = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Cat")
        {
            inRange = true;
            cat = other.GetComponent<BaseNPC>();
            talkTo = other;
            catCollide = other;
        }
        else if (other.tag == "Customer")
        {
            inRange = true;
            customer = other.GetComponent<BaseNPC>();
            npcTarget = other.gameObject;
            talkTo = other;
            custCollide = other;
        }
        else if (other.tag == "Minigame")
        {
            startPlay = true;
            talkTo = other;
            gameCollide = other;
            Debug.Log("q to start");
        }
    }

    private void OnTriggerExit2D(Collider2D leavingOther)
    {
        if (leavingOther.tag == "Cat")
        {
            inRange = false;
            cat = leavingOther.GetComponent<BaseNPC>();
            talkTo = null;
            catCollide = null;
        }
        else if (leavingOther.tag == "Customer")
        {
            inRange = false;
            customer = leavingOther.GetComponent<BaseNPC>();
            npcTarget = leavingOther.gameObject;
            talkTo = null;
            custCollide = null;
        }
        else if (leavingOther.tag == "Minigame")
        {
            startPlay = false;
            talkTo = null;
            gameCollide = null;
        }
    }
}
