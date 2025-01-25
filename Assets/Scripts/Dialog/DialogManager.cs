using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;

public class DialogManager : MonoBehaviour
{

    [Header("Dialog UI")]

    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public bool IsPlaying { get; private set; } = false;

    static Story story;
    Text nametag;
    Text message;
    List<string> tags;

    public GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    static Choice choiceSelected;

    private static DialogManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
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

    public static DialogManager GetInstance()
    {
        return instance;
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
    }

    /*** Tag Parser ***/
    /// In Inky, you can use tags which can be used to cue stuff in a game.
    /// This is just one way of doing it. Not the only method on how to trigger events. 
    // void ParseTags()
    // {
    //     tags = story.currentTags;
    //     foreach (string t in tags)
    //     {
    //         string prefix = t.Split(' ')[0];
    //         string param = t.Split(' ')[1];

    //         switch (prefix.ToLower())
    //         {
    //             case "anim":
    //                 SetAnimation(param);
    //                 break;
    //             case "color":
    //                 SetTextColor(param);
    //                 break;
    //         }
    //     }
    // }
    // void SetAnimation(string _name)
    // {
    //     CharacterScript cs = FindObjectOfType<CharacterScript>();
    //     cs.PlayAnimation(_name);
    // }
    // void SetTextColor(string _color)
    // {
    //     switch (_color)
    //     {
    //         case "red":
    //             message.color = Color.red;
    //             break;
    //         case "blue":
    //             message.color = Color.cyan;
    //             break;
    //         case "green":
    //             message.color = Color.green;
    //             break;
    //         case "white":
    //             message.color = Color.white;
    //             break;
    //         default:
    //             Debug.Log($"{_color} is not available as a text color");
    //             break;
    //     }
    // }

}