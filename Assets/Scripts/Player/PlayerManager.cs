using System.Collections;
using System.Collections.Generic;
using KevinCastejon.FiniteStateMachine;
using Pathfinding;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerScript Player;

    [SerializeField]
    private PlayerFSM PlayerFSM;

    private Rigidbody2D rb;

    int selectNum = 0;
    AIDestinationSetter currCat = null;
    GameObject currNPC;

    public bool idling = true;
    public bool walking = false;
    public bool playing = false;
    public bool talking = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = Player.rb;
    }

    // Update is called once per frame
    void Update()
    {
        //unpaused
        if (Time.timeScale != 0)
        {
            //checking input
            bool keyIn = Input.anyKey;
            bool conIn = Input.GetButton("Fire1");
            bool joyIn =
                Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f
                || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

            if (!keyIn && !conIn && !joyIn)
            {
                idling = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                idling = false;
                walking = false;
                talking = true;
            }
            else
            {
                talking = false;
                idling = true;
            }

            if (!playing && !talking && Input.GetKey(KeyCode.Space))
            {
                if (Player.inRange && Player.talkTo != null)
                {
                    matchTint(Player.talkTo);
                }
                else
                {
                    selectNum = 0;
                }
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
            }
        }
    }

    private void FixedUpdate() { }

    private void matchTint(Collider2D npc)
    {
        if (npc.tag == "Cat" && selectNum == 0)
        {
            selectNum = 1;
            Player.npcSprite.color = Color.red;
            currNPC = Player.npc;
            currCat = Player.npcTarget;
        }
        else if (npc.tag == "Customer" && selectNum == 1)
        {
            selectNum = 2;
            Player.npcSprite.color = Color.red;
            currNPC = Player.npc;
            currCat.target = currNPC.transform;
        }
    }
}
