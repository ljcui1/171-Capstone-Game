using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualIndicator : MonoBehaviour
{
    [SerializeField] private GameObject talkative;
    [SerializeField] private GameObject active;
    [SerializeField] private GameObject foodie;

    public void SetActive(bool toggle, Attribute? attribute = null)
    {
        try
        {
            gameObject.SetActive(toggle);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error setting gameObject active: " + ex.Message);
        }

        if (talkative == null && active == null && foodie == null)
        {
            Debug.LogWarning("Set visual indicators for " + transform.parent.gameObject.name);
            return;
        }

        if (attribute.HasValue)
        {
            if (attribute == Attribute.Talkative)
            {
                talkative.SetActive(true);
                active.SetActive(false);
                foodie.SetActive(false);
            }
            else if (attribute == Attribute.Foodie)
            {
                talkative.SetActive(false);
                active.SetActive(false);
                foodie.SetActive(true);
            }
            else if (attribute == Attribute.Active)
            {
                talkative.SetActive(false);
                active.SetActive(true);
                foodie.SetActive(false);
            }
        }
        else
        {
            talkative.SetActive(false);
            active.SetActive(false);
            foodie.SetActive(false);
        }
    }

}
