using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// copied from https://youtu.be/sQpjgcX-jH8?si=m0FhX_igrULcUfpF
public class SetsUiElementToSelectOnInteraction : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable elementToSelect;

    [Header("Visualization")]
    [SerializeField] private bool showVisualization;
    [SerializeField] private Color navigationColour = Color.cyan;

    private void OnDrawGizmos()
    {
        if (!showVisualization)
            return;

        if (elementToSelect == null)
            return;

        Gizmos.color = navigationColour;
        Gizmos.DrawLine(gameObject.transform.position, elementToSelect.gameObject.transform.position);
    }

    private void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
            Debug.Log("Did not find an Event System in your Scene.", this);
    }

    public void FindEventSystem()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
            Debug.Log("Did not find an Event System in your Scene.", this);
    }

    public void JumpToElement()
    {
        if (eventSystem == null)
            Debug.Log("This item has no event system referenced yet.", this);

        if (elementToSelect == null)
            Debug.Log("This should jump where?", this);

        Debug.Log("Jumping to next button");
        eventSystem.SetSelectedGameObject(elementToSelect.gameObject);
    }
}