using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RememberGameScene : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject lastSelectedElement;
    [SerializeField] private GameObject startDayButton;
    [SerializeField] private DialogManager dialogManager;

    private void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        if (!eventSystem)
        {
            Debug.Log("Did not find an Event System in this scene.", this);
            return;
        }

        lastSelectedElement = eventSystem.firstSelectedGameObject;
    }

    private void Update()
    {
        if (!eventSystem)
            return;

        if (eventSystem.currentSelectedGameObject &&
            lastSelectedElement != eventSystem.currentSelectedGameObject)
        {
            lastSelectedElement = eventSystem.currentSelectedGameObject;
        }

        if (eventSystem.currentSelectedGameObject && !dialogManager.IsPlaying &&
        (eventSystem.currentSelectedGameObject.name == "Choice0" ||
        eventSystem.currentSelectedGameObject.name == "Choice1") &&
        startDayButton.activeSelf)
        {
            lastSelectedElement = startDayButton;
        }

        if (lastSelectedElement && (!eventSystem.currentSelectedGameObject || lastSelectedElement != eventSystem.currentSelectedGameObject))
        {
            eventSystem.SetSelectedGameObject(lastSelectedElement);
            Debug.Log(eventSystem.currentSelectedGameObject);
        }
    }
}
