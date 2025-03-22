using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;
using System;

// taken from https://github.com/shapedbyrainstudios/ink-dialogue-system/blob/8-ink-external-functions-example/Assets/Scripts/Dialogue/DialogueManager.cs 
// Credit:  shapedbyrainstudios https://www.youtube.com/watch?v=vY0Sk93YUhA 
public class DialogManager : MonoBehaviour
{

    [Header("Dialog UI")]
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    private DialogScript currentDialogScript;
    [SerializeField] private EventSystem eventSystem;

    public GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private const string ATTRIBUTE_TAG = "attribute";
    private const string AFFINITY_TAG = "affinity";

    public GameObject ContinueIcon;

    public bool IsPlaying { get; private set; } = false;

    private static Story story;
    private static DialogManager instance;

    public Action OnDialogueEnd; // Event to notify when dialogue ends


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        IsPlaying = false;
        dialogPanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        ContinueIcon.SetActive(true);

        int index = 0;
        foreach (GameObject choice in choices)
        {
            choices[index].SetActive(false);
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }
    private void Update()
    {
        if (IsPlaying)
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonDown("ButtonX"))
            {
                // If choices are active, select the currently highlighted choice
                if (story.currentChoices.Count > 0)
                {
                    GameObject selectedChoice = eventSystem.currentSelectedGameObject;
                    int choiceIndex = 0;
                    if (selectedChoice != null)
                    {
                        // Find the index of the currently selected button
                        for (int i = 0; i < choices.Length; i++)
                        {
                            if (choices[i] == selectedChoice)
                            {
                                choiceIndex = i;
                                break;
                            }
                        }
                    }
                    MakeChoice(choiceIndex);
                }
                else
                {
                    ContinueStory();
                }
            }

            // Update ContinueIcon visibility:
            // Show the continue icon if there is either a narrative line waiting or choices available
            if (story.canContinue || story.currentChoices.Count > 0)
            {
                ContinueIcon.SetActive(true);
            }
            else
            {
                // No narrative and no choices available: we are at the end of dialogue, so hide the icon.
                ContinueIcon.SetActive(false);
            }
        }

    }

    public void HandlePanelClick()
    {
        Debug.Log("Clicked on dialog panel");
        if (IsPlaying && story.currentChoices.Count == 0)
        {
            if (!story.canContinue)
            {
                ExitDialogMode();
            }
            else
            {
                ContinueStory();
            }
        }
    }



    public void EnterDialogMode(TextAsset inkJSON, DialogScript dialogScript)
    {
        story = new Story(inkJSON.text);
        dialogPanel.SetActive(true);
        currentDialogScript = dialogScript; // Store the reference

        IsPlaying = true;
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (story.canContinue)
        {
            AdvanceDialog();
            DisplayChoices();
            // handle tags
            HandleTags(story.currentTags);
        }
        else
        {
            ExitDialogMode();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            // parse tag
            string[] splitTag = tag.Split(":");
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be properly parsed" + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle tag
            switch (tagKey)
            {
                case ATTRIBUTE_TAG:
                    //Debug.Log("attribute=" + tagValue);
                    break;
                case AFFINITY_TAG:
                    //Debug.Log("affinity=" + tagValue);
                    break;
                default:
                    //Debug.LogWarning("Tag Key is currently not being handled" + tag);
                    break;
            }

        }
    }

    // Advance through the story 
    void AdvanceDialog()
    {
        string currentSentence = story.Continue(); // Continue() grabs next line in script
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    private void ExitDialogMode()
    {
        IsPlaying = false;
        dialogPanel.SetActive(false);
        dialogText.text = "";
        if (currentDialogScript != null)
        {
            currentDialogScript.MarkDialogueAsPlayed(); // Call only the relevant DialogScript
            currentDialogScript = null; // Reset reference
        }

    }


    // Type out the sentence letter by letter and make character idle if they were talking
    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
        yield return null;
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = story.currentChoices;
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError($"More choices given than the UI can support. {currentChoices.Count} choices given.");
        }
        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // hide the leftover button choices
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        Debug.Log("CHOICES[0S]" + choices[0].gameObject);
        eventSystem.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        story.ChooseChoiceIndex(choiceIndex);
        // NOTE: The below two lines were added to fix a bug after the Youtube video was made
        // this is specific to my InputManager script
        ContinueStory();
    }

}
