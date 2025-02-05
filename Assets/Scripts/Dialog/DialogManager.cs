using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;

// taken from https://github.com/shapedbyrainstudios/ink-dialogue-system/blob/8-ink-external-functions-example/Assets/Scripts/Dialogue/DialogueManager.cs 
// Credit:  shapedbyrainstudios https://www.youtube.com/watch?v=vY0Sk93YUhA 
public class DialogManager : MonoBehaviour
{

    [Header("Dialog UI")]
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    public GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    public bool IsPlaying { get; private set; } = false;

    private static Story story;
    private static DialogManager instance;

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
        if (!IsPlaying) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }


    public void EnterDialogMode(TextAsset inkJSON)
    {
        story = new Story(inkJSON.text);
        IsPlaying = true;
        dialogPanel.SetActive(true);

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (story.canContinue)
        {
            AdvanceDialog();
            if (story.currentChoices.Count != 0)
            {
                DisplayChoices();
            }
        }
        else
        {
            ExitDialogMode();
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
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    // // Tells the story which branch to go to
    public void SetDecision(int choiceIndex)
    {
        story.ChooseChoiceIndex(choiceIndex);
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }
        ContinueStory();
    }

}