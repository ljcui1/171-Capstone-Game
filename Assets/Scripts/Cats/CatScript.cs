using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    [SerializeField] private bool walk = true;
    [SerializeField] private bool sit = false;
    [SerializeField] private bool talk = false;
    [SerializeField] private bool bond = false;
    [SerializeField] private bool reject = false;
    [SerializeField] private bool accept = false;

    [SerializeField] private List<CatAttribute> activeAttributes = new List<CatAttribute>();

    // Enum to define possible attributes
    public enum CatAttribute
    {
        Talkative,
        Foodie,
        Active
    }

    private void Start()
    {
        HandleAttributes();
    }

    // Handle the active attributes
    private void HandleAttributes()
    {
        foreach (var attribute in activeAttributes)
        {
            Debug.Log("Active attribute: " + attribute.ToString());
            // Implement logic based on active attributes
            if (attribute == CatAttribute.Talkative)
            {
                // Example logic: make the cat talk
            }
            if (attribute == CatAttribute.Foodie)
            {
                // Example logic: make the cat hungry
            }
            if (attribute == CatAttribute.Active)
            {
                // Example logic: make the cat active
            }
        }
    }

}
