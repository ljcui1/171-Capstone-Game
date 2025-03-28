using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

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

    public GameManager gameMan;

    [SerializeField]
    public TextMeshProUGUI InteractText;

    [SerializeField]
    public TextMeshProUGUI MatchText;

    [SerializeField]
    public TextMeshProUGUI TalkText;

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
    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            Debug.Log("Gamepad");
            InteractText.text = "press A to play";
            MatchText.text = "press B to match";
            TalkText.text = "press A to talk";
        }
        else if (Keyboard.current.wasUpdatedThisFrame || Mouse.current.wasUpdatedThisFrame)
        {
            // Debug.Log("Keyboard");
            InteractText.text = "press E to play";
            MatchText.text = "press SPACE to match";
            TalkText.text = "press E to talk";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Minigame")
        {
            startPlay = true;
            talkTo = other;
            gameCollide = other;
            Debug.Log("e to start");
            InteractText.enabled = gameMan.nightOrDay == GameManager.NightOrDay.DAY;
        }
        else if (other.tag == "Customer")
        {
            inRange = true;
            customer = other.GetComponent<BaseNPC>();
            npcTarget = other.gameObject;
            talkTo = other;
            custCollide = other;
            MatchText.enabled = gameMan.nightOrDay == GameManager.NightOrDay.DAY;
            TalkText.enabled = gameMan.nightOrDay == GameManager.NightOrDay.DAY;
        }
        else if (other.tag == "Cat")
        {
            inRange = true;
            cat = other.GetComponent<BaseNPC>();
            talkTo = other;
            catCollide = other;
            MatchText.enabled = gameMan.nightOrDay == GameManager.NightOrDay.DAY;
            TalkText.enabled = gameMan.nightOrDay == GameManager.NightOrDay.NIGHT;
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
        else if (leavingOther.tag == "Cat")
        {
            inRange = false;
            cat = leavingOther.GetComponent<BaseNPC>();
            talkTo = null;
            catCollide = null;
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
