using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[System.Serializable]


public class DialogScript : MonoBehaviour
{
    public Collider2D triggerZone;
    public KeyCode interactKey = KeyCode.E;

    [SerializeField] protected TextAsset defaultText;

    [SerializeField] public VisualIndicator visualCue;

    [SerializeField] protected GameManager gameManager;

    [SerializeField] private BaseNPC npc;

    [EnumFlags]
    public TimeOfDay timeOfDay;

    public bool CUSTOMER = false;

    public List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();
    private List<DialogueEntry> customerDialog = new List<DialogueEntry>();
    protected bool isPlayerInZone = false;

    // This could be set by the NPC or some game logic
    protected Attribute? selectedAttribute;
    protected TextAsset selectedText;
    protected DialogueEntry currentDialogue; // Store the dialogue being played

    void Awake()
    {

        visualCue = GetComponentInChildren<VisualIndicator>();
        visualCue.SetActive(false);

        npc = GetComponent<BaseNPC>();
        // Debug.Log(npc);

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
        customerDialog = dialogueEntries;
    }

    void Start()
    {
        if (CUSTOMER)
        {
            CustomerScript custscript = GetComponent<CustomerScript>();

            dialogueEntries = customerDialog.FindAll(entry => custscript.attributes.Any(attrPair => attrPair.attribute == entry.attribute));
            // set dialogentries played to isActive in attributes
            foreach (DialogueEntry entry in dialogueEntries)
            {
                entry.played = custscript.attributes.Find(attrPair => attrPair.attribute == entry.attribute).isActive;
            }

        }

        SelectRandomText();
    }

    public void SetEntryPlayed()
    {
        // set dialogentries played to isActive in attributes
        foreach (DialogueEntry entry in dialogueEntries)
        {
            Debug.Log("Setting played to isActive " + gameObject.name);
            entry.played = npc.attributes.Find(attrPair => attrPair.attribute == entry.attribute).isActive;
            Debug.Log(entry.played);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
        SelectRandomText();
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
            Attribute[] playedAttributes = dialogueEntries.FindAll(entry => entry.played).ConvertAll(entry => entry.attribute).ToArray();
            // Debug.Log(playedAttributes);
            if (selectedText != defaultText || playedAttributes.Length > 0)
            {
                visualCue.SetActive(true, playedAttributes);
            }
            if (Input.GetKeyDown(interactKey) || Input.GetButtonDown("ButtonA"))
            {
                Debug.Log("Interacting with " + gameObject.name);
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
            // Store the attribute from the current dialogue
            Attribute dialogueAttribute = currentDialogue.attribute;
            currentDialogue.played = true;

            // Set the NPC's attribute based on the current dialogue
            Debug.Log($"Setting {dialogueAttribute} active");
            npc.SetAttribute(dialogueAttribute, true);
        }

        // Now select the next dialogue (this may result in defaultText with no attribute)
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
#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
    }
}

#endif
public enum TimeOfDay
{
    Day = 1 << 0,
    Night = 1 << 1
}
