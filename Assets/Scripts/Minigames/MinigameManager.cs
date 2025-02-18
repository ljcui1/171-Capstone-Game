using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance { get; private set; }

    public List<BaseMinigame> games;

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
            Debug.Log(instance);
        }
    }

    void Start()
    {
        foreach (BaseMinigame game in games)
        {
            game.enabled = false;
        }
    }

    public void StartMinigame(Attribute attribute)
    {
        foreach (BaseMinigame game in games)
        {
            if (game.attribute == attribute)
            {
                game.enabled = true;
                game.StartGame();
                return;
            }
        }

        Debug.Log("Failed to find minigame matching found attribute");
    }

    public void StopMinigame()
    {
        foreach (BaseMinigame game in games)
        {
            if (game.enabled == true)
            {
                game.GameOver();
                game.enabled = false;
                Debug.Log("Stoping " + game);
            }
        }

    }

    public void GameScore(float score, float maxScore, Attribute attribute)
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
}
