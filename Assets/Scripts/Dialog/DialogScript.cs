using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public Collider2D triggerZone;
    public KeyCode interactKey = KeyCode.E;
    public string character = "Fluffy";

    [SerializeField] private GameObject visualCue;

    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private bool isPlayerInZone = false;


    void Start()
    {
        visualCue.SetActive(false);
        isPlayerInZone = false;
        triggerZone = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log($"press {interactKey} to interact");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            // trigger dialog to close
            Debug.Log("exit");
        }
    }

    void Update()
    {
        if (isPlayerInZone && !DialogManager.GetInstance().IsPlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(interactKey))
            {
                Debug.Log($"{interactKey} key pressed: Start dialog interaction");
                Debug.Log($"Interacted with {character}");
                DialogManager.GetInstance().EnterDialogMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }

    }
}
