using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public Collider2D triggerZone;
    public KeyCode interactKey = KeyCode.E;

    [SerializeField] private GameObject visualCue;

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
                if (dialogueEntries.Count > 0)
                {
                    // selects a random entry to start -- should be modified
                    int randomIndex = Random.Range(0, dialogueEntries.Count);
                    DialogueEntry randomDialogue = dialogueEntries[randomIndex];
                    DialogManager.GetInstance().EnterDialogMode(randomDialogue.textAsset);
                }
                else
                {
                    Debug.LogWarning("No dialogues available in the dictionary.");
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
}
