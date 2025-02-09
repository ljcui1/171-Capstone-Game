using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance { get; private set; }

    public MiceGame mouse;

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
}
