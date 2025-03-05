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
    // AIDestinationSetter currCat = null;
    // GameObject currNPC;

    public bool idling = true;
    public bool walking = false;
    public bool playing = false;
    public bool talking = false;

    public bool joyIn = false;

    private Collider2D talkTo;

    private BaseNPC selectedCat = null;
    private BaseNPC selectedCust = null;
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

            // Play walking sound if moving
            if (joyIn && !playing && !talking)
            {
                idling = false;
                walking = true;
                AudioManager.Instance.PlayWalkingSound(true);
            }
            else
            {
                walking = false;
                AudioManager.Instance.StopWalkingSound(true);
            }

            if (!keyIn && !conIn && !joyIn)
            {
                idling = true;
                AudioManager.Instance.StopWalkingSound(true);
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

            if (!playing && !talking && Input.GetKeyDown(KeyCode.Space))
            {
                // Debug.Log("space clicked");
                //check if player can interact with an npc
                if (Player.inRange && Player.talkTo != null)
                {
                    // Debug.Log("enter match");
                    //MatchTint(Player.talkTo);
                    //talkTo = Player.talkTo;
                    if (Player.catCollide != null)
                    {
                        MatchTint(Player.catCollide);
                    }
                    if (Player.custCollide != null)
                    {
                        MatchTint(Player.custCollide);
                    }
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
            if (Player.talkTo == null)
            {
                Debug.LogWarning("talkto is null");
            }

        }
    }

    private void FixedUpdate() { }

    private void MatchTint(Collider2D npc)
    {
        if (npc.CompareTag("Cat"))
        {
            if (selectedCat == null) // First selection
            {
                Debug.Log("Selecting cat: " + npc.name);
                selectedCat = npc.GetComponent<BaseNPC>();
                selectedCat.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (selectedCat == npc.GetComponent<BaseNPC>()) // Deselect if pressed again
            {
                Debug.Log("Deselecting cat: " + npc.name);
                selectedCat.GetComponent<SpriteRenderer>().color = Color.white;
                selectedCat = null;
            }
        }
        else if (npc.CompareTag("Customer") && selectedCat != null)
        {
            if (selectedCust == null) // Customer selection
            {
                Debug.Log("Selecting customer: " + npc.name);
                selectedCust = npc.GetComponent<BaseNPC>();
                selectedCust.GetComponent<SpriteRenderer>().color = Color.red;

                // Move the cat to the customer
                selectedCat.GetComponent<AIDestinationSetter>().target = selectedCust.transform;

                // Check match conditions
                if (CheckMatch(selectedCat, selectedCust))
                {
                    Debug.Log("Match found! Moving to the door.");
                    /*Transform door = GameObject.Find("Door").transform;
                    selectedCat.GetComponent<AIDestinationSetter>().target = door;
                    selectedCust.GetComponent<AIDestinationSetter>().target = door;*/
                }
                else
                {
                    Debug.Log("No match found.");
                }
            }
        }
    }

    private bool CheckMatch(BaseNPC cat, BaseNPC customer)
    {
        if (cat == null || customer == null) return false;

        // Extract active attributes
        HashSet<Attribute> catAttributes = GetActiveAttributes(cat);
        HashSet<Attribute> customerAttributes = GetActiveAttributes(customer);

        // Compare sets (return true if they have the same attributes)
        return catAttributes.SetEquals(customerAttributes);
    }

    private HashSet<Attribute> GetActiveAttributes(MonoBehaviour npc)
    {
        HashSet<Attribute> activeAttributes = new HashSet<Attribute>();
        var npcAttributes = npc.GetType().GetProperty("attributes").GetValue(npc) as List<AttributePair>;

        if (npcAttributes != null)
        {
            foreach (var attr in npcAttributes)
            {
                if (attr.isActive)
                {
                    activeAttributes.Add(attr.attribute);
                }
            }
        }
        return activeAttributes;
    }

    /*private void MatchTint(Collider2D npc)
    {
        Debug.Log("tag" + npc.tag);
        if (npc.tag == "Cat" && selectNum == 0)
        {
            Debug.Log("select cat " + npc);
            //set select to 1
            selectNum = 1;
            //tint sprite color/highlight
            Player.cat.mainSprite.color = Color.red;

            /*if(selectNum == 0) 
            {
            Debug.Log("select cat " + npc);
            //set select to 1
            selectNum = 1;
            //tint sprite color/highlight
            Player.cat.mainSprite.color = Color.red;
            }else if(selectNum == 1)
            {
            Debug.Log("deselect cat " + npc);
            //set select to 1
            selectNum = 0;
            //tint sprite color/highlight
            Player.cat.mainSprite.color = Color.white;
            }//
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
    }*/
}
