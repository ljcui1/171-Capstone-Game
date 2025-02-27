using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEditor;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public Collider2D triggerZone;
    public KeyCode interactKey = KeyCode.E;

    [SerializeField] protected TextAsset defaultText;

    [SerializeField] public VisualIndicator visualCue;

    [SerializeField] protected GameManager gameManager;

    [EnumFlags]
    public TimeOfDay timeOfDay;

    public List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();
    protected bool isPlayerInZone = false;

    // This could be set by the NPC or some game logic
    protected Attribute? selectedAttribute;
    protected TextAsset selectedText;
    protected DialogueEntry currentDialogue; // Store the dialogue being played

    void Awake()
    {
        visualCue.SetActive(false);
        isPlayerInZone = false;
        triggerZone = GetComponent<Collider2D>();

        if (dialogueEntries.Count == 0)
        {
            Debug.LogWarning("No dialog available for this object", transform.parent.gameObject);
        }
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("gameManager not found for " + transform.gameObject.name);
        }
    }

    void Start()
    {
        SelectRandomText();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    void Update()
    {
        if (isPlayerInZone && !DialogManager.GetInstance().IsPlaying)
        {
            visualCue.SetActive(true, selectedAttribute);
            if (Input.GetKeyDown(interactKey))
            {
                //Plays talking sound when selecting a cat
                AudioManager.Instance.PlayTalkingSound();
                if (selectedText)
                {
                    DialogManager.GetInstance().EnterDialogMode(selectedText, this);

                }
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    public void SelectRandomText()
    {
        // Filter the dialogues to only those that haven't been played yet
        List<DialogueEntry> availableDialogues = dialogueEntries.FindAll(entry => !entry.played);
        if (availableDialogues.Count > 0 && CanPlay())
        {
            int randomIndex = UnityEngine.Random.Range(0, availableDialogues.Count);
            currentDialogue = availableDialogues[randomIndex]; // Store reference
            selectedAttribute = currentDialogue.attribute;
            selectedText = currentDialogue.textAsset;
        }
        else
        {
            // All dialogues have been played. Use default text if available.
            if (defaultText == null)
            {
                selectedText = null;
                Debug.LogWarning("No dialogues available and no default text assigned.");
            }
            else
            {
                currentDialogue = null;
                selectedAttribute = null;
                selectedText = defaultText;
            }
        }
    }

    public void MarkDialogueAsPlayed()
    {
        if (currentDialogue != null)
        {
            currentDialogue.played = true;
            // Debug.Log("Marked dialogue as played: " + currentDialogue.textAsset.name);
        }
        SelectRandomText();
    }

    public bool CanPlay()
    {
        // Check if the current time in the game matches the dialogue's allowed time
        TimeOfDay currentTime = gameManager.CurrentTime();

        // Use bitwise AND to check if the dialogue's time of day matches the current time
        return (timeOfDay & currentTime) != 0;
    }

}


[System.Serializable]
public class DialogueEntry
{
    public Attribute attribute; // Use the same enum defined in BaseNPC
    public TextAsset textAsset; // Corresponding dialogue JSON file

    public bool played = false;

    // [EnumFlags] // Custom attribute to show checkboxes in Inspector
    // public DaysOfWeek daysOfTheWeekActive;
}


// from https://chatgpt.com/share/67bac8e1-e5b0-8000-93e6-7c5137d6a81f
public class EnumFlagsAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
    }
}

[Flags]
public enum DaysOfWeek
{
    Monday = 1 << 0,
    Tuesday = 1 << 1,
    Wednesday = 1 << 2,
    Thursday = 1 << 3,
    Friday = 1 << 4,
    Saturday = 1 << 5,
    Sunday = 1 << 6
}

public enum TimeOfDay
{
    Day = 1 << 0,
    Night = 1 << 1
}
