using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance { get; private set; }
    public Canvas mouseGame;

    [SerializeField]
    private Image pawa;

    [SerializeField]
    private Image paws;

    [SerializeField]
    private Image pawd;

    [SerializeField]
    private Image pawf;

    [Header("Customer")]
    [SerializeField] private CustomerManager CustomerMan;

    [Header("Point Values")]
    [SerializeField] private List<float> scorePercentage;
    [SerializeField] private List<int> customersToAdd;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseGame.enabled = false;
    }

    // Update is called once per frame
    void Update() { }

    public void GameScore(int score, int maxScore, Attribute attribute)
    {
        for (int i = 0; i < scorePercentage.Count; i++)
        {
            if (score / maxScore >= scorePercentage[i])
            {
                CustomerMan.AddCustomerProbability(customersToAdd[i], customersToAdd[i] / 100, attribute);
                return;
            }
        }
    }

    public void MouseMiniGamePlay()
    {
        mouseGame.enabled = true;
        /*pawa.enabled = false;
        paws.enabled = false;
        pawd.enabled = false;
        pawf.enabled = false;*/
        if (Input.GetKey(KeyCode.A))
        {
            pawa.enabled = true;
            paws.enabled = false;
            pawd.enabled = false;
            pawf.enabled = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            pawa.enabled = false;
            paws.enabled = true;
            pawd.enabled = false;
            pawf.enabled = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            pawa.enabled = false;
            paws.enabled = false;
            pawd.enabled = true;
            pawf.enabled = false;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            pawa.enabled = false;
            paws.enabled = false;
            pawd.enabled = false;
            pawf.enabled = true;
        }
    }
}
