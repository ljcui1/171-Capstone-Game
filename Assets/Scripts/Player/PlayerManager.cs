using System.Collections;
using System.Collections.Generic;
using KevinCastejon.FiniteStateMachine;
using Pathfinding;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public PlayerScript Player;
    public CatManager CatMan;

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

    [SerializeField] private BaseNPC selectedCat = null;
    [SerializeField] private BaseNPC selectedCust = null;

    [SerializeField] private GameObject doorObj;
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

            if (!playing && !talking && Input.GetKeyDown(KeyCode.Space) && Player.gameMan.nightOrDay == GameManager.NightOrDay.DAY)
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
                        Debug.Log("CatSpace");
                        MatchTint(Player.catCollide);
                    }
                    if (Player.custCollide != null)
                    {
                        Debug.Log("CustomerSpace");
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
        /*else
        {
            if (Player.talkTo == null)
            {
                Debug.LogWarning("talkto is null");
            }

        }*/

        if (talking)
        {
            Player.TalkText.enabled = false;
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
        if (npc.CompareTag("Customer") && selectedCat != null)
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
                    Transform door = doorObj.transform;
                    selectedCat.GetComponent<AIDestinationSetter>().target = door;
                    selectedCust.GetComponent<AIDestinationSetter>().target = door;
                    if (selectedCat.transform == door && selectedCust.transform == door)
                    {
                        selectedCat.enabled = false;
                        selectedCust.enabled = false;
                    }
                }
                else
                {
                    Debug.Log("No match found.");
                    selectedCat.GetComponent<SpriteRenderer>().color = Color.white;
                    selectedCust.GetComponent<SpriteRenderer>().color = Color.white;
                    CatMan.SetRandomLocation(selectedCat.gameObject);
                }
                selectedCat = null;
                selectedCust = null;
            }
        }
    }

    private bool CheckMatch(BaseNPC cat, BaseNPC customer)
    {
        if (cat == null || customer == null) return false;

        var catAttributes = new HashSet<Attribute>(
            cat.GetComponent<CatScript>().attributes.Where(attr => attr.isActive).Select(attr => attr.attribute)
        );

        var customerAttributes = new HashSet<Attribute>(
            customer.attributes.Where(attr => attr.isActive).Select(attr => attr.attribute)
        );

        Debug.Log($"Checking match...");
        Debug.Log($"Cat Attributes ({cat.name}): {string.Join(", ", catAttributes)}");
        Debug.Log($"Customer Attributes ({customer.name}): {string.Join(", ", customerAttributes)}");

        bool match = catAttributes.SetEquals(customerAttributes);
        Debug.Log($"Match Result: {match}");

        return match;
    }
}
