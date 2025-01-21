using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class CustomerManager : MonoBehaviour
{
    // Customer spawn chance
    [SerializeField] private List<string> attributes;   // all available attributes
    public List<float> attributeWeights;                // customer attribute probability
    [SerializeField] private int numAttributes;         // how many attributes customers have
    public int numCustomersToSpawn;                     // num set by mini games
    [SerializeField] private float minWait;             // wait time for spawn
    [SerializeField] private float maxWait;             // wait time for spawn

    // customer pool - code referenced from https://learn.unity.com/tutorial/introduction-to-object-pooling
    private List<GameObject> customerPool;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private int amountToPool;
    public GameObject entrance;

    // Cafe chairs
    [SerializeField] private List<GameObject> chair;
    private List<bool> chairOccupied;

    // Start is called before the first frame update
    void Start()
    {
        // Set up customer pool
        customerPool = new List<GameObject>();
        GameObject tmpCustomer;
        for (int i = 0; i < amountToPool; i++)
        {
            tmpCustomer = Instantiate(customerPrefab, transform);
            tmpCustomer.SetActive(false);
            customerPool.Add(tmpCustomer);
        }

        // set chairs as unoccupied
        chairOccupied = new();
        for (int i = 0; i < chair.Count; i++)
        {
            chairOccupied.Add(false);
        }
    }

    public GameObject GetPooledCustomer()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!customerPool[i].activeInHierarchy)
            {
                return customerPool[i];
            }
        }
        return null;
    }

    private float SumOfWeights()
    {
        float sum = 0f;
        foreach (float weight in attributeWeights)
        {
            sum += weight;
        }
        return sum;
    }

    private void SelectAttribute(List<string> attr, List<string> restOfAttr, List<float> restOfWeight)
    {
        float randomNum = Random.Range(0f, SumOfWeights());
        float cumWeight = 0f;
        for (int i = 0; i < restOfAttr.Count; i++)
        {
            cumWeight += restOfWeight[i];
            if (randomNum <= cumWeight)
            {
                attr.Add(restOfAttr[i]);
                // removes selected attribute from list so it's not selected again
                restOfAttr.RemoveAt(i);
                restOfWeight.RemoveAt(i);
                return;
            }
        }
    }

    private GameObject SelectDestination()
    {
        for (int i = 0; i < chair.Count; i++)
        {
            if (!chairOccupied[i])
            {
                chairOccupied[i] = true;
                return chair[i];
            }
        }

        return null;
    }

    private void SpawnCustomer()
    {
        // check if there's unoccupied seats, if not don't spawn
        GameObject chair = SelectDestination();
        if (chair == null)
        {
            return;
        }

        // Select attributes for customers
        List<string> attr = new();
        List<string> restOfAttr = new(attributes);
        List<float> restOfWeight = new(attributeWeights);
        for (int i = 0; i < numAttributes; i++)
        {
            SelectAttribute(attr, restOfAttr, restOfWeight);
        }

        // spawn in customer at entrance
        GameObject customer = GetPooledCustomer();
        if (customer != null)
        {
            CustomerScript script = customer.GetComponent<CustomerScript>();
            script.SetAttributes(attr);
            script.SetDestination(chair);
            customer.transform.position = entrance.transform.position;
            customer.SetActive(true);
        }
    }

    // Generate customers with random wait times between customers (Called by GameManager)
    public IEnumerator CustomerWave()
    {
        float waitTime = Random.Range(minWait, maxWait);

        for (int i = 0; i < numCustomersToSpawn; i++)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(waitTime);
        }
    }


}
