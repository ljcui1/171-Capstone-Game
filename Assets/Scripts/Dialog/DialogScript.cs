using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public Collider2D triggerZone;
    public KeyCode interactKey = KeyCode.E;

    [SerializeField] protected TextAsset defaultText;

    [SerializeField] public VisualIndicator visualCue;

    [SerializeField] protected GameManager gameManager;

    public List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();
    protected bool isPlayerInZone = false;

    // This could be set by the NPC or some game logic
    protected Attribute? selectedAttribute;
    protected TextAsset selectedText;
    protected DialogueEntry currentDialogue; // Store the dialogue being played


    void Start()
    {
        visualCue.SetActive(false);
        isPlayerInZone = false;
        triggerZone = GetComponent<Collider2D>();

        if (dialogueEntries.Count == 0)
        {
            Debug.LogWarning("No dialog available for this object", transform.parent.gameObject);
        }

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        DialogManager.GetInstance().OnDialogueEnd += MarkDialogueAsPlayed;
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
                if (selectedText)
                {
                    DialogManager.GetInstance().EnterDialogMode(selectedText);

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
        if (availableDialogues.Count > 0)
        {
            int randomIndex = Random.Range(0, availableDialogues.Count);
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
                selectedAttribute = null;
                selectedText = defaultText;
            }
        }
    }

    void MarkDialogueAsPlayed()
    {
        if (currentDialogue != null)
        {
            currentDialogue.played = true;
            Debug.Log("Marked dialogue as played: " + currentDialogue.textAsset.name);
        }
        SelectRandomText();
    }

}


[System.Serializable]
public class DialogueEntry
{
    public Attribute attribute; // Use the same enum defined in BaseNPC
    public TextAsset textAsset; // Corresponding dialogue JSON file

    public bool played = false;
}
