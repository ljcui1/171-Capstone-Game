using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    // Customer spawn chance
    [SerializeField] private List<GameManager.Attribute> attributes;   // all available attributes
    public List<float> attributeWeights;                // customer attribute probability
    [SerializeField] private int numAttributes;         // how many attributes customers have
    public int numCustomersToSpawn;                     // num set by mini games
    [SerializeField] private float minWait;             // wait time for spawn
    [SerializeField] private float maxWait;             // wait time for spawn
    [SerializeField] private int minHours; // Minimum amount of time in the cafe
    [SerializeField] private int maxHours; // Maximum amount of time in the cafe

    // Object pool for customers
    private List<GameObject> customerPool;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private int amountToPool;
    public GameObject entrance; // Entrance point for customers

    // Cafe chairs
    [SerializeField] private List<GameObject> chairs;
    private List<bool> chairOccupied = new List<bool>();

    void Start()
    {
        // Initialize customer pool
        customerPool = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject tmpCustomer = Instantiate(customerPrefab, transform);
            tmpCustomer.SetActive(false);
            customerPool.Add(tmpCustomer);
        }

        // Initialize chair occupancy
        for (int i = 0; i < chairs.Count; i++)
        {
            chairOccupied.Add(false);
        }
    }

    private GameObject GetPooledCustomer()
    {
        foreach (var customer in customerPool)
        {
            Debug.Log(customer + " " + customer.activeSelf);
            if (!customer.activeSelf)
            {
                return customer;
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

    private void SelectAttribute(List<Attribute> attr, List<Attribute> remainingAttr, List<float> remainingWeights)
    {
        float randomNum = Random.Range(0f, SumOfWeights());
        float cumulativeWeight = 0f;

        for (int i = 0; i < remainingAttr.Count; i++)
        {
            cumulativeWeight += remainingWeights[i];
            if (randomNum <= cumulativeWeight)
            {
                attr.Add(remainingAttr[i]);
                remainingAttr.RemoveAt(i);
                remainingWeights.RemoveAt(i);
                return;
            }
        }
    }

    private GameObject SelectDestination()
    {
        for (int i = 0; i < chairs.Count; i++)
        {
            if (!chairOccupied[i])
            {
                chairOccupied[i] = true;
                return chairs[i];
            }
        }

        return null;
    }

    private void SpawnCustomer()
    {
        // Check for available chair
        GameObject chair = SelectDestination();
        if (chair == null)
        {
            Debug.Log("No unoccupied chairs available");
            return;
        }

        // Generate customer attributes
        List<Attribute> attr = new();
        List<Attribute> remainingAttr = new(attributes);
        List<float> remainingWeights = new(attributeWeights);

        for (int i = 0; i < numAttributes; i++)
        {
            SelectAttribute(attr, remainingAttr, remainingWeights);
        }

        // Spawn customer
        GameObject customer = GetPooledCustomer();
        if (customer != null)
        {
            CustomerScript script = customer.GetComponent<CustomerScript>();
            script.SetAttributes(attr);
            script.SetDestination(chair);
            customer.transform.position = entrance.transform.position;
            customer.SetActive(true);
        }
        else
        {
            Debug.Log("Failed to get pooled customer");
        }
    }

    public IEnumerator CustomerWave()
    {
        for (int i = 0; i < numCustomersToSpawn; i++)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        }
    }

    private void SendCustomerOut(CustomerScript customer)
    {
        customer.walkout = true;
    }

    public void CustomerHours()
    {
        foreach (var customer in customerPool)
        {
            // if customer is in the cafe then they've been there for +1 hour
            if (customer.activeSelf)
            {
                CustomerScript script = customer.GetComponent<CustomerScript>();
                script.hourStayed++;

                // send customer out if they've been here for EX. 1 or 2 hours
                if (script.hourStayed >= Random.Range(minHours, maxHours))
                {
                    SendCustomerOut(script);
                }
            }
        }
    }

    public void ClosedCustomersLeave()
    {
        foreach (var customer in customerPool)
        {
            // if customer is in the cafe then they've been there for +1 hour
            if (customer.activeSelf)
            {
                SendCustomerOut(customer.GetComponent<CustomerScript>());
            }
        }
    }
}
