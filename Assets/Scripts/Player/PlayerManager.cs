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

    bool connecting = false;

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
                    matchLine(Player.talkTo.transform.position);
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

    private void matchLine(Vector2 sPos)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            connecting = true;
            Debug.Log("line start");
            Vector2 startPos = new Vector3(sPos.x, sPos.y, 0);
            Player.lr.SetPosition(0, startPos);
            Player.lr.SetPosition(1, startPos);
            Player.lr.enabled = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("line inprogress");
            Vector2 endPos = new Vector3(
                Player.transform.position.x,
                Player.transform.position.y,
                0
            );
            Player.lr.SetPosition(1, endPos);
        }
        /*if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Player.inRange && Player.talkTo != null && Player.talkTo.tag == "Customer")
            {
                connecting = false;
                Debug.Log("line end");
                Vector3 customerPos = new Vector3(
                    Player.talkTo.transform.position.x,
                    Player.talkTo.transform.position.y,
                    0
                );
                Player.lr.SetPosition(1, customerPos);
            }
            else
            {
                Debug.Log("no target");
                Player.lr.enabled = false;
            }
        }*/
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Waiting for target...");

            // Don't disable the line immediately, just mark the connection as pending.
            connecting = false;

            StartCoroutine(WaitForSecondNPC());
        }
    }

    // Coroutine to wait for second NPC connection
    private IEnumerator WaitForSecondNPC()
    {
        float waitTime = 1.5f; // Allow some time to find the second NPC
        float timer = 0f;

        while (timer < waitTime)
        {
            if (Player.inRange && Player.talkTo != null && Player.talkTo.tag == "Customer")
            {
                Debug.Log("Line connected to Customer!");
                Vector3 customerPos = new Vector3(
                    Player.talkTo.transform.position.x,
                    Player.talkTo.transform.position.y,
                    0
                );
                Player.lr.SetPosition(1, customerPos);
                yield break; // Exit coroutine early if a Customer is found
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // If no customer is found after wait time, disable line
        Debug.Log("No target found, disabling line.");
        Player.lr.enabled = false;
    }
}
