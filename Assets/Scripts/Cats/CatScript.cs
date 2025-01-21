using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;
public class CatScript : MonoBehaviour
{
    [SerializeField] private bool walk = true;
    [SerializeField] private bool sit = false;
    [SerializeField] private bool talk = false;
    [SerializeField] private bool bond = false;
    [SerializeField] private bool reject = false;
    [SerializeField] private bool accept = false;

    [SerializeField] private List<GameManager.Attribute> activeAttributes = new List<GameManager.Attribute>();

    public AIDestinationSetter aiDestinationSetter;

    private void Awake()
    {
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        if (aiDestinationSetter == null)
        {
            Debug.LogError($"{name} is missing the AIDestinationSetter component!");
        }
    }
    private void Start()
    {
        HandleAttributes();
    }

    public void SetPathTarget(GameObject location)
    {
        aiDestinationSetter.target = location.transform;
    }

    // Handle the active attributes
    private void HandleAttributes()
    {
        foreach (var attribute in activeAttributes)
        {
            Debug.Log("Active attribute: " + attribute.ToString());
            // Implement logic based on active attributes
            if (attribute == GameManager.Attribute.Talkative)
            {
                // Example logic: make the cat talk
            }
            if (attribute == GameManager.Attribute.Foodie)
            {
                // Example logic: make the cat hungry
            }
            if (attribute == GameManager.Attribute.Active)
            {
                // Example logic: make the cat active
            }
        }
    }

}
