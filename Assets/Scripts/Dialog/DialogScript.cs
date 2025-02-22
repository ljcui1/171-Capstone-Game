using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public Collider2D triggerZone;
    public KeyCode interactKey = KeyCode.E;

    [SerializeField] TextAsset defaultText;

    [SerializeField] private GameObject visualCue;

    [SerializeField] private GameManager gameManager;

    public List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();
    private bool isPlayerInZone = false;

    // This could be set by the NPC or some game logic
    private Attribute selectedAttribute;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
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
            visualCue.SetActive(true);
            if (Input.GetKeyDown(interactKey))
            {
                // Filter the dialogues to only those that haven't been played yet
                List<DialogueEntry> availableDialogues = dialogueEntries.FindAll(entry => !entry.played);

                // commented out for testing
                // if (availableDialogues.Count > 0 && gameManager.IsNightTime())
                if (availableDialogues.Count > 0)
                {
                    int randomIndex = Random.Range(0, availableDialogues.Count);
                    DialogueEntry randomDialogue = availableDialogues[randomIndex];
                    randomDialogue.played = true;
                    DialogManager.GetInstance().EnterDialogMode(randomDialogue.textAsset);
                }
                else
                {
                    // All dialogues have been played. Use default text if available.
                    if (defaultText == null)
                    {
                        Debug.LogWarning("No dialogues available and no default text assigned.");
                    }
                    else
                    {
                        DialogManager.GetInstance().EnterDialogMode(defaultText);
                    }
                }
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

}

[System.Serializable]
public class DialogueEntry
{
    public Attribute attribute; // Use the same enum defined in BaseNPC
    public TextAsset textAsset; // Corresponding dialogue JSON file

    public bool played = false;
}
