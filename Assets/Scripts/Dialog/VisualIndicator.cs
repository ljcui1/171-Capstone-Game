using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualIndicator : MonoBehaviour
{
    // pretty sure this is the worst way to go around displaying attributes but i'm lazy
    [SerializeField] private GameObject talkative;
    [SerializeField] private GameObject active;
    [SerializeField] private GameObject foodie;

    [SerializeField] private GameObject activeTalkative;
    [SerializeField] private GameObject activeFoodie;
    [SerializeField] private GameObject foodieTalkative;
    [SerializeField] private GameObject all;

    public void SetActive(bool toggle, Attribute[] attributes = null)
    {
        // Set the parent GameObject's active state.
        try
        {
            gameObject.SetActive(toggle);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error setting gameObject active: " + ex.Message);
        }

        // Reset all indicators.
        talkative.SetActive(false);
        active.SetActive(false);
        foodie.SetActive(false);
        activeTalkative.SetActive(false);
        activeFoodie.SetActive(false);
        foodieTalkative.SetActive(false);

        // If no attributes are passed in, exit early.
        if (attributes == null || attributes.Length == 0)
        {
            return;
        }

        // Flags for each attribute.
        bool hasActive = false;
        bool hasTalkative = false;
        bool hasFoodie = false;

        // Loop through each attribute in the array.
        foreach (var attr in attributes)
        {
            if (attr == Attribute.Active)
                hasActive = true;
            else if (attr == Attribute.Talkative)
                hasTalkative = true;
            else if (attr == Attribute.Foodie)
                hasFoodie = true;
        }

        // Check for combinations first.
        if (hasActive && hasTalkative && !hasFoodie)
        {
            activeTalkative.SetActive(true);
        }
        else if (hasActive && hasFoodie && !hasTalkative)
        {
            activeFoodie.SetActive(true);
        }
        else if (hasTalkative && hasFoodie && !hasActive)
        {
            foodieTalkative.SetActive(true);
        }
        // If all three attributes are present, you might want to handle it differently.
        // For this example, we'll activate all individual indicators.
        else if (hasActive && hasTalkative && hasFoodie)
        {
            all.SetActive(true);
        }
        // If only one attribute is true, activate its indicator.
        else if (hasActive)
        {
            active.SetActive(true);
        }
        else if (hasTalkative)
        {
            talkative.SetActive(true);
        }
        else if (hasFoodie)
        {
            foodie.SetActive(true);
        }
    }
}
