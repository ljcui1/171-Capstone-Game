using System.Collections;
using System.Collections.Generic;
using KevinCastejon.FiniteStateMachine;
using Pathfinding;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerScript Player;

    public MinigameManager MiniMan;

    [SerializeField]
    private PlayerFSM PlayerFSM;

    private Rigidbody2D rb;

    int selectNum = 0;
    // AIDestinationSetter currCat = null;
    // GameObject currNPC;

    public bool idling = true;
    public bool walking = false;
    public bool playing = false;
    public bool talking = false;

    public bool joyIn = false;

    private Collider2D talkTo;
    // Start is called before the first frame update
    void Start()
    {
        rb = Player.rb;
    }

    // Update is called once per frame
    void Update()
    {
        //unpaused
        if (Time.timeScale != 0f)
        {
            playing = false;
            //checking input
            bool keyIn = Input.anyKey;
            bool conIn = Input.GetButton("Fire1");
            joyIn =
                Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f
                || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

            if (!keyIn && !conIn && !joyIn)
            {
                idling = true;
            }

            /*if (Input.GetKeyDown(KeyCode.E))
            {
                idling = false;
                walking = false;
                talking = true;
            }
            else
            {
                talking = false;
                idling = true;
            }*/

            if (!playing && !talking && Input.GetKey(KeyCode.Space))
            {
                Debug.Log("space clicked");
                //check if player can interact with an npc
                if (Player.inRange && Player.talkTo != null)
                {
                    Debug.Log("enter match");
                    MatchTint(Player.talkTo);
                    talkTo = Player.talkTo;
                }
                //else, set select to 0 to reset process
                else
                {
                    selectNum = 0;
                    // currCat = null;
                    // currNPC = null;
                }
            }

            /*if (Player.startPlay && Input.GetKey(KeyCode.Q))
            {
                idling = false;
                walking = false;
                playing = true;
            }

            //checking if playing & talking are false and movement input is given to put player into walking state
            if (joyIn && !playing && !talking)
            {
                idling = false;
                walking = true;
            }
            else
            {
                walking = false;
            }*/
        }
        else
        {
            if (!talking)
            {
                idling = false;
                walking = false;
                playing = true;
            }
        }
    }

    private void FixedUpdate() { }

    private void MatchTint(Collider2D npc)
    {
        Debug.Log("tag" + npc.tag);
        if (npc.tag == "Cat" && selectNum == 0)
        {
            Debug.Log("select cat " + npc);
            //set select to 1
            selectNum = 1;
            //tint sprite color/highlight
            Player.cat.mainSprite.color = Color.red;
        }
        else if (npc.tag == "Customer" && selectNum == 1)
        {
            Debug.Log("select customer " + npc);
            //set select to 1
            selectNum = 2;
            //tint sprite color/highlight
            Player.customer.mainSprite.color = Color.red;
            //set cat target to customer
            Player.cat.SetDestination(Player.npcTarget);
        }
    }

    /*private void matchTint(Collider2D npc)
{
    if (npc.tag == "Cat" && selectNum == 0)
    {
        Debug.Log("select cat");
        selectNum = 1;

        if (Player.npcSprite != null)
        {
            Player.npcSprite.color = Color.red;
        }
        else
        {
            Debug.LogError("Player.npcSprite is not assigned!");
        }

        currCat = Player.npcTarget;

        if (currCat == null)
        {
            Debug.LogError("No valid cat assigned!");
        }
    }
    else if (npc.tag == "Customer" && selectNum == 1)
    {
        Debug.Log("select customer");
        selectNum = 2;

        if (Player.npcSprite != null)
        {
            Player.npcSprite.color = Color.green;
        }
        else
        {
            Debug.LogError("Player.npcSprite is not assigned!");
        }

        currNPC = Player.npc;

        if (currCat != null && currNPC != null)
        {
            currCat.target = currNPC.transform;
        }
        else
        {
            Debug.LogError("currCat or currNPC is not assigned!");
        }
    }
}*/
}
