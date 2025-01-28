using System.Collections;
using System.Collections.Generic;
using KevinCastejon.FiniteStateMachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerScript Player;

    [SerializeField]
    private PlayerFSM PlayerFSM;

    private Rigidbody2D rb;

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

            if (Player.inRange && Input.GetKeyDown(KeyCode.E))
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
                matchLine();
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

    private void matchLine()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("line start");
            Vector2 startPos = Player.transform.position;
            Player.lr.SetPosition(0, startPos);
            Player.lr.SetPosition(1, startPos);
            Player.lr.enabled = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("line inprogress");
            Vector2 endPos = Player.transform.position;
            Player.lr.SetPosition(1, endPos);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("line end");
            Player.lr.enabled = false;
        }
    }
}
