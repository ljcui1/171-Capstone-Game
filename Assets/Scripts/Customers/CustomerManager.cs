using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    // Customer spawn parameters
    [Header("Spawn Parameters")]
    [SerializeField] private List<Attribute> attributes; // All available attributes
    //public List<float> attributeWeights; // Attribute probabilities
    private Dictionary<Attribute, float> attributeWeights = new Dictionary<Attribute, float>(); // Attribute probabilities
    [SerializeField] private int numAttributes; // Number of attributes per customer
    public int numCustomersToSpawn; // Number of customers to spawn


    [Header("Spawn Delay")]
    [SerializeField] private float minWait; // Minimum wait time for spawn
    [SerializeField] private float maxWait; // Maximum wait time for spawn

    [Header("Cafe Stay")]
    [SerializeField] private int minHours; // Minimum amount of time in the cafe
    [SerializeField] private int maxHours; // Maximum amount of time in the cafe

    // Object pool for customers
    [Header("Object Pool")]

    [SerializeField] private GameObject customerPrefab;
    private List<GameObject> customerPool;
    [SerializeField] private int amountToPool;

    [Header("Destinations")]
    public GameObject entrance; // Entrance point for customers
    // Cafe chairs
    [SerializeField] private List<GameObject> chairs;
    private List<bool> chairOccupied = new List<bool>();

    void Start()
    {
        // populate attributeWeights with all possible attributes and set to 0s
        foreach (Attribute att in attributes)
        {
            attributeWeights.Add(att, 0);
        }

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

    public void AddCustomerProbability(int addCustomer, float addweight, Attribute attribute)
    {
        numCustomersToSpawn += addCustomer;
        attributeWeights[attribute] = attributeWeights[attribute] + addweight;
    }

    private GameObject GetPooledCustomer()
    {
        foreach (var customer in customerPool)
        {
            if (!customer.activeSelf)
            {
                return customer;
            }
        }
        return null;
    }

    private float SumOfWeights(Dictionary<Attribute, float> weights)
    {
        float sum = 0f;
        foreach (KeyValuePair<Attribute, float> att in weights)
        {
            sum += att.Value;
        }
        return sum;
    }

    private void SelectAttribute(List<Attribute> attr, Dictionary<Attribute, float> remainingAttr)
    {
        float randomNum = Random.Range(0f, SumOfWeights(remainingAttr));
        float cumulativeWeight = 0f;
        Debug.Log("Start Selection");
        foreach (KeyValuePair<Attribute, float> remaining in remainingAttr)
        {
            cumulativeWeight += remaining.Value;
            Debug.Log(remaining.Key + " " + cumulativeWeight + " " + randomNum);
            if (randomNum <= cumulativeWeight)
            {
                attr.Add(remaining.Key);
                remainingAttr.Remove(remaining.Key);
                return;
            }
        }
    }

    private int SelectDestination()
    {
        // check for available chairs
        for (int i = 0; i < chairs.Count; i++)
        {
            if (!chairOccupied[i])
            {
                // if any chair is available then randomly choose chairs
                while (true)
                {
                    int rand = Random.Range(i, chairOccupied.Count);
                    if (!chairOccupied[rand])
                    {
                        chairOccupied[rand] = true;
                        return rand;
                    }
                }
            }
        }

        return -1;
    }

    private void SpawnCustomer()
    {
        // Check for available chair
        int chairIndex = SelectDestination();
        if (chairIndex == -1)
        {
            Debug.Log("No unoccupied chairs available");
            return;
        }
        GameObject chair = chairs[chairIndex];

        // Generate customer attributes
        List<Attribute> attr = new();
        Dictionary<Attribute, float> remainingAttr = new(attributeWeights);
        Debug.Log($"Before selection: {string.Join(", ", remainingAttr.Keys)}");
        for (int i = 0; i < numAttributes; i++)
        {
            SelectAttribute(attr, remainingAttr);
            Debug.Log($"After selection: {string.Join(", ", remainingAttr.Keys)}");
        }

        // Spawn customer
        GameObject customer = GetPooledCustomer();
        if (customer != null)
        {
            Debug.Log(attributeWeights[Attribute.Talkative] + " " + attributeWeights[Attribute.Foodie] + " " + attributeWeights[Attribute.Active]);
            CustomerScript script = customer.GetComponent<CustomerScript>();
            script.SetAttributes(attr);
            script.SetDestination(chair);
            customer.transform.position = entrance.transform.position;
            customer.SetActive(true);
            script.chair = chairIndex;
            AudioManager.Instance.PlayEnterChime(); //Should play enter chime here
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

        numCustomersToSpawn /= 3;
    }

    private void SendCustomerOut(CustomerScript customer)
    {
        customer.walkout = true;
        chairOccupied[customer.chair] = false;
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

    internal void AddCustomerProbability(int v1, double v2, Attribute attribute)
    {
        throw new System.NotImplementedException();
    }
}
