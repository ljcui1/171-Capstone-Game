using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

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

    private void Update()
    {
        if (!IsPlaying) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
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
        // CharacterScript tempSpeaker = FindObjectOfType<CharacterScript>();
        // if (tempSpeaker.IsPlaying)
        // {
        //     SetAnimation("idle");
        // }
        yield return null;
    }

    // // Create then show the choices on the screen until one got selected
    // IEnumerator ShowChoices()
    // {
    //     Debug.Log("There are choices need to be made here!");
    //     List<Choice> _choices = story.currentChoices;

    //     for (int i = 0; i < _choices.Count; i++)
    //     {
    //         GameObject temp = Instantiate(customButton, optionPanel.transform);
    //         temp.transform.GetChild(0).GetComponent<Text>().text = _choices[i].text;
    //         temp.AddComponent<Selectable>();
    //         temp.GetComponent<Selectable>().element = _choices[i];
    //         temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().Decide(); });
    //     }

    //     optionPanel.SetActive(true);

    //     yield return new WaitUntil(() => { return choiceSelected != null; });

    //     AdvanceFromDecision();
    // }

    // // Tells the story which branch to go to
    public static void SetDecision(object element)
    {
        choiceSelected = (Choice)element;
        story.ChooseChoiceIndex(choiceSelected.index);
    }

    // // After a choice was made, turn off the panel and advance from that choice
    // void AdvanceFromDecision()
    // {
    //     optionPanel.SetActive(false);
    //     for (int i = 0; i < optionPanel.transform.childCount; i++)
    //     {
    //         Destroy(optionPanel.transform.GetChild(i).gameObject);
    //     }
    //     choiceSelected = null; // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.
    //     AdvanceDialog();
    // }

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