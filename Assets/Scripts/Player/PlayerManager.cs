// using System.Collections;
// using System.Collections.Generic;
// using KevinCastejon.FiniteStateMachine;
// using Pathfinding;
// using UnityEngine;
// using System.Linq;

// public class PlayerManager : MonoBehaviour
// {
//     public PlayerScript Player;
//     public CatManager CatMan;

//     public MinigameManager MiniMan;

//     [SerializeField]
//     private PlayerFSM PlayerFSM;

//     private Rigidbody2D rb;
//     // AIDestinationSetter currCat = null;
//     // GameObject currNPC;

//     public bool idling = true;
//     public bool walking = false;
//     public bool playing = false;
//     public bool talking = false;

//     public bool joyIn = false;

//     private Collider2D talkTo;

//     [SerializeField] private CatScript selectedCat = null;
//     [SerializeField] private CustomerScript selectedCust = null;

//     [SerializeField] private GameObject doorObj;
//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = Player.rb;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //unpaused
//         if (Time.timeScale != 0f)
//         {
//             playing = false;
//             //checking input
//             bool keyIn = Input.anyKey;
//             bool conIn = Input.GetButton("Fire1");
//             joyIn =
//                 Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f
//                 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

//             // Play walking sound if moving
//             if (joyIn && !playing && !talking)
//             {
//                 idling = false;
//                 walking = true;
//                 AudioManager.Instance.PlayWalkingSound(true);
//             }
//             else
//             {
//                 walking = false;
//                 AudioManager.Instance.StopWalkingSound(true);
//             }

//             if (!keyIn && !conIn && !joyIn)
//             {
//                 idling = true;
//                 AudioManager.Instance.StopWalkingSound(true);
//             }

//             /*if (Input.GetKeyDown(KeyCode.E))
//             {
//                 idling = false;
//                 walking = false;
//                 talking = true;
//             }
//             else
//             {
//                 talking = false;
//                 idling = true;
//             }*/

//             if (!playing && !talking && Input.GetKeyDown(KeyCode.Space) && Player.gameMan.nightOrDay == GameManager.NightOrDay.DAY)
//             {
//                 // Debug.Log("space clicked");
//                 //check if player can interact with an npc
//                 if (Player.inRange && Player.talkTo != null)
//                 {
//                     // Debug.Log("enter match");
//                     //MatchTint(Player.talkTo);
//                     //talkTo = Player.talkTo;
//                     if (Player.catCollide != null)
//                     {
//                         Debug.Log("CatSpace");
//                         MatchTint(Player.catCollide);
//                     }
//                     if (Player.custCollide != null)
//                     {
//                         Debug.Log("CustomerSpace");
//                         MatchTint(Player.custCollide);
//                     }
//                 }
//             }

//             /*if (Player.startPlay && Input.GetKey(KeyCode.Q))
//             {
//                 idling = false;
//                 walking = false;
//                 playing = true;
//             }

//             //checking if playing & talking are false and movement input is given to put player into walking state
//             if (joyIn && !playing && !talking)
//             {
//                 idling = false;
//                 walking = true;
//             }
//             else
//             {
//                 walking = false;
//             }*/
//         }
//         /*else
//         {
//             if (Player.talkTo == null)
//             {
//                 Debug.LogWarning("talkto is null");
//             }

//         }*/

//         if (talking)
//         {
//             Player.TalkText.enabled = false;
//             Player.MatchText.enabled = false;
//             Player.InteractText.enabled = false;
//         }
//     }

//     private void FixedUpdate() { }

//     private void MatchTint(Collider2D npc)
//     {
//         if (npc.CompareTag("Cat"))
//         {
//             if (selectedCat == null) // First selection
//             {
//                 Debug.Log("Selecting cat: " + npc.name);
//                 selectedCat = npc.GetComponent<CatScript>();
//                 selectedCat.GetComponent<SpriteRenderer>().color = Color.red;
//             }
//             else if (selectedCat == npc.GetComponent<CatScript>()) // Deselect if pressed again
//             {
//                 Debug.Log("Deselecting cat: " + npc.name);
//                 selectedCat.GetComponent<SpriteRenderer>().color = Color.white;
//                 selectedCat = null;
//             }
//         }
//         if (npc.CompareTag("Customer") && selectedCat != null)
//         {
//             if (selectedCust == null) // Customer selection
//             {
//                 Debug.Log("Selecting customer: " + npc.name);
//                 selectedCust = npc.GetComponent<CustomerScript>();
//                 selectedCust.GetComponent<SpriteRenderer>().color = Color.red;

//                 // Move the cat to the customer
//                 selectedCat.GetComponent<AIDestinationSetter>().target = selectedCust.transform;

//                 // Check match conditions
//                 if (CheckMatch(selectedCat, selectedCust))
//                 {
//                     Debug.Log("Match found! Moving to the door.");
//                     Transform door = doorObj.transform;
//                     selectedCat.GetComponent<SpriteRenderer>().color = Color.green;
//                     selectedCust.GetComponent<SpriteRenderer>().color = Color.green;
//                     selectedCat.matched = true;
//                     selectedCat.SetDestination(doorObj);
//                     selectedCust.SetDestination(doorObj);
//                     StartCoroutine(WaitForLeaving(selectedCat, selectedCust, door));
//                 }
//                 else
//                 {
//                     Debug.Log("No match found.");
//                     selectedCat.GetComponent<SpriteRenderer>().color = Color.white;
//                     selectedCust.GetComponent<SpriteRenderer>().color = Color.white;
//                     CatScript cat = selectedCat.gameObject.GetComponent<CatScript>();
//                     cat.SetRandomLocation();
//                 }
//                 selectedCat = null;
//                 selectedCust = null;
//             }
//         }
//     }

//     private bool CheckMatch(BaseNPC cat, BaseNPC customer)
//     {
//         if (cat == null || customer == null) return false;

//         var catAttributes = new HashSet<Attribute>(
//             cat.GetComponent<CatScript>().attributes.Where(attr => attr.isActive).Select(attr => attr.attribute)
//         );

//         var customerAttributes = new HashSet<Attribute>(
//             customer.GetComponent<CustomerScript>().attributes.Where(attr => attr.isActive).Select(attr => attr.attribute)
//         );

//         Debug.Log($"Checking match...");
//         Debug.Log($"Cat Attributes ({cat.name}): {string.Join(", ", catAttributes)}");
//         Debug.Log($"Customer Attributes ({customer.name}): {string.Join(", ", customerAttributes)}");

//         // Check if customer has at least all attributes of cat, and if customer has at least 2 attributes from the cat's list
//         bool hasRequiredAttributes = catAttributes.All(attribute => customerAttributes.Contains(attribute));
//         bool hasMinAttributes = catAttributes.Count() <= customerAttributes.Count() && customerAttributes.Intersect(catAttributes).Count() >= 2;

//         bool match = hasRequiredAttributes && hasMinAttributes;
//         Debug.Log($"Match Result: {match}");

//         return match;
//     }

//     private IEnumerator WaitForLeaving(BaseNPC cat, BaseNPC customer, Transform door)
//     {
//         cat.GetComponent<AIPath>().endReachedDistance = 0.1f;
//         customer.GetComponent<AIPath>().endReachedDistance = 0.1f;
//         while (!cat.GetComponent<AIPath>().reachedEndOfPath || !customer.GetComponent<AIPath>().reachedEndOfPath)
//         {
//             yield return null;
//         }
//         yield return new WaitForSeconds(0.1f);
//         cat.GetComponent<SpriteRenderer>().color = Color.white;
//         customer.GetComponent<SpriteRenderer>().color = Color.white;
//         cat.gameObject.SetActive(false);
//         customer.gameObject.SetActive(false);
//     }
// }

// Refactored code but still has issues with the MatchTint method

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using Pathfinding;

public class PlayerManager : MonoBehaviour
{
    public PlayerScript Player;
    public CatManager CatMan;
    public MinigameManager MiniMan;

    [SerializeField] private PlayerFSM PlayerFSM;
    [SerializeField] private GameObject doorObj;
    [SerializeField] private CatScript selectedCat = null;
    [SerializeField] private CustomerScript selectedCust = null;

    private Rigidbody2D rb;
    public bool idling = true;
    public bool walking = false;
    public bool playing = false;
    public bool talking = false;
    public bool joyIn = false;

    private void Start()
    {
        rb = Player.rb;
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return; // Skip update when paused

        HandleInput();
        HandleMovementAudio();
        HandleInteraction();
        UpdateUI();
    }

    private void HandleInput()
    {
        bool keyIn = Input.anyKey;
        bool conIn = Input.GetButton("Fire1");
        joyIn = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        idling = !keyIn && !conIn && !joyIn;
        walking = joyIn && !playing && !talking;
    }

    private void HandleMovementAudio()
    {
        if (walking)
            AudioManager.Instance.PlayWalkingSound(true);
        else
            AudioManager.Instance.StopWalkingSound(true);
    }

    private void HandleInteraction()
    {
        if (!playing && !talking && Input.GetKeyDown(KeyCode.Space) && Player.gameMan.nightOrDay == GameManager.NightOrDay.DAY)
        {
            if (Player.inRange && Player.talkTo != null)
            {
                if (Player.catCollide != null)
                {
                    MatchTint(Player.catCollide);
                }
                else if (Player.custCollide != null)
                {
                    MatchTint(Player.custCollide);
                }
            }
        }
    }

    private void UpdateUI()
    {
        if (talking)
        {
            Player.TalkText.enabled = false;
            Player.MatchText.enabled = false;
            Player.InteractText.enabled = false;
        }
    }

    private void MatchTint(Collider2D npc)
    {
        if (npc.CompareTag("Cat"))
        {
            ToggleCatSelection(npc.GetComponent<CatScript>());
        }
        else if (npc.CompareTag("Customer") && selectedCat != null)
        {
            ProcessCustomerSelection(npc.GetComponent<CustomerScript>());
        }
    }

    private void ToggleCatSelection(CatScript cat)
    {
        if (selectedCat == null)
        {
            selectedCat = cat;
            cat.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (selectedCat == cat)
        {
            selectedCat.GetComponent<SpriteRenderer>().color = Color.white;
            selectedCat = null;
        }
    }

    private void ProcessCustomerSelection(CustomerScript customer)
    {
        selectedCust = customer;
        customer.GetComponent<SpriteRenderer>().color = Color.red;
        selectedCat.GetComponent<AIDestinationSetter>().target = customer.transform;

        if (CheckMatch(selectedCat, selectedCust))
        {
            CompleteMatch(selectedCat, selectedCust);
        }
        else
        {
            ResetSelection();
        }
    }

    private void CompleteMatch(CatScript cat, CustomerScript customer)
    {
        Transform door = doorObj.transform;
        cat.GetComponent<SpriteRenderer>().color = Color.green;
        customer.GetComponent<SpriteRenderer>().color = Color.green;
        cat.matched = true;
        cat.SetDestination(doorObj);
        customer.walkout = true;
        StartCoroutine(WaitForLeaving(cat, customer, door));
        selectedCat = null;
        selectedCust = null;
    }

    private void ResetSelection()
    {
        selectedCat.GetComponent<SpriteRenderer>().color = Color.white;
        selectedCust.GetComponent<SpriteRenderer>().color = Color.white;
        selectedCat.SetRandomLocation();
        selectedCat = null;
        selectedCust = null;
    }

    private bool CheckMatch(BaseNPC cat, BaseNPC customer)
    {
        if (cat == null || customer == null) return false;

        var catAttributes = cat.GetComponent<CatScript>().attributes.Where(attr => attr.isActive).Select(attr => attr.attribute).ToHashSet();
        var customerAttributes = customer.GetComponent<CustomerScript>().attributes.Where(attr => attr.isActive).Select(attr => attr.attribute).ToHashSet();

        return catAttributes.All(attribute => customerAttributes.Contains(attribute)) && customerAttributes.Intersect(catAttributes).Count() >= 2;
    }

    private IEnumerator WaitForLeaving(BaseNPC cat, BaseNPC customer, Transform door)
    {
        Debug.Log("Before wait ");
        while (!cat.AtDestination())
        {
            yield return null;
        }
        Debug.Log("After wait ");
        /*cat.GetComponent<AIPath>().endReachedDistance = 0.1f;
        customer.GetComponent<AIPath>().endReachedDistance = 0.1f;
        Debug.Log("Before wait " + cat.GetComponent<AIDestinationSetter>().target + " " + cat.GetComponent<AIPath>().reachedEndOfPath);
        while (!cat.GetComponent<AIPath>().reachedEndOfPath || !customer.GetComponent<AIPath>().reachedEndOfPath)
        {
            yield return null;
        }
        Debug.Log("after wait " + cat.GetComponent<AIDestinationSetter>().target + " " + cat.GetComponent<AIPath>().reachedEndOfPath);
        yield return new WaitForSeconds(0.1f);
        */
        cat.GetComponent<SpriteRenderer>().color = Color.white;

        cat.gameObject.SetActive(false);
        //customer.gameObject.SetActive(false);
    }
}

