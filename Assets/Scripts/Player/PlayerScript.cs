using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

    [SerializeField] private Image Overlay;
    [SerializeField] private TextMeshProUGUI InteractText;
    [SerializeField] private TextMeshProUGUI MatchText;
    [SerializeField] private TextMeshProUGUI TalkText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anims = GetComponent<Animator>();
        InteractText.enabled = false;
        MatchText.enabled = false;
        TalkText.enabled = false;
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Minigame")
        {
            startPlay = true;
            talkTo = other;
            gameCollide = other;
            Debug.Log("e to start");
            InteractText.enabled = true;
        }
        else if (other.tag == "Cat")
        {
            inRange = true;
            cat = other.GetComponent<BaseNPC>();
            talkTo = other;
            catCollide = other;
            MatchText.enabled = true;
            TalkText.enabled = !Overlay.enabled;
        }
        else if (other.tag == "Customer")
        {
            inRange = true;
            customer = other.GetComponent<BaseNPC>();
            npcTarget = other.gameObject;
            talkTo = other;
            custCollide = other;
            MatchText.enabled = true;
            TalkText.enabled = Overlay.enabled;
        }
    }

    private void OnTriggerExit2D(Collider2D leavingOther)
    {
        if (leavingOther.tag == "Minigame")
        {
            startPlay = false;
            talkTo = null;
            gameCollide = null;
            InteractText.enabled = false;
        }
        else if (leavingOther.tag == "Cat")
        {
            inRange = false;
            cat = leavingOther.GetComponent<BaseNPC>();
            talkTo = null;
            catCollide = null;
            MatchText.enabled = false;
            TalkText.enabled = false;
        }
        else if (leavingOther.tag == "Customer")
        {
            inRange = false;
            customer = leavingOther.GetComponent<BaseNPC>();
            npcTarget = leavingOther.gameObject;
            talkTo = null;
            custCollide = null;
            MatchText.enabled = false;
            TalkText.enabled = false;
        }

        if (gameCollide)
        {
            talkTo = gameCollide;
            startPlay = true;
        }
        else if (custCollide)
        {
            talkTo = custCollide;
            inRange = true;
        }
        else if (custCollide)
        {
            talkTo = custCollide;
            inRange = true;
        }
    }
}
