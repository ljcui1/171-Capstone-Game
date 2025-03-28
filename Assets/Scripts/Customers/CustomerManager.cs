using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance { get; private set; }

    // Customer spawn parameters
    [Header("Spawn Parameters")]
    public List<Attribute> attributes; // All available attributes
    public Dictionary<Attribute, float> attributeWeights = new Dictionary<Attribute, float>(); // Attribute probabilities
    // Removed numAttributes since we're always selecting two attributes
    public int numCustomersToSpawn; // Number of customers to spawn

    [Header("Spawn Delay")]
    [SerializeField] private float minWait; // Minimum wait time for spawn
    [SerializeField] private float maxWait; // Maximum wait time for spawn

    [Header("Cafe Stay")]
    [SerializeField] private int minHours; // Minimum amount of time in the cafe
    [SerializeField] private int maxHours; // Maximum amount of time in the cafe

    // Object pool for customers
    [Header("Object Pool")]
    public GameObject customerPrefab;
    public List<CustomerScript> customerPool;
    //[SerializeField] private int amountToPool;

    [Header("Destinations")]
    public GameObject entrance; // Entrance point for customers
    // Cafe chairs
    public List<GameObject> chairs;
    public List<bool> chairOccupied = new List<bool>();

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

    void OnApplicationQuit()
    {
        SaveScript.SaveCustSpawnData();
        SaveScript.SaveCustomers();
    }

    void Start()
    {
        // Populate attributeWeights with all possible attributes and set their initial weight (e.g., 0)
        foreach (Attribute att in attributes)
        {
            attributeWeights.Add(att, 0);
        }

        // Initialize customer pool
        customerPool = new List<CustomerScript>();
        // Uncomment below to pre-instantiate a pool of customers if desired
        // for (int i = 0; i < amountToPool; i++)
        // {
        //     GameObject tmpCustomer = Instantiate(customerPrefab, transform);
        //     tmpCustomer.SetActive(false);
        //     customerPool.Add(tmpCustomer);
        // }

        // Initialize chair occupancy
        for (int i = 0; i < chairs.Count; i++)
        {
            chairOccupied.Add(false);
        }

        SaveScript.LoadCustSpawnData();
        SaveScript.LoadCustomer();
    }

    public void AddCustomerProbability(int addCustomer, float addweight, Attribute attribute)
    {
        numCustomersToSpawn += addCustomer;
        attributeWeights[attribute] += addweight;
    }

    private CustomerScript GetPooledCustomer()
    {
        foreach (var customer in customerPool)
        {
            if (!customer.gameObject.activeSelf)
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

    /// <summary>
    /// Selects exactly two attributes from the remainingAttr dictionary.
    /// If the total weight is zero, selection is made randomly.
    /// Otherwise, weighted selection is used.
    /// Selected attributes are removed from the dictionary to avoid duplicates.
    /// </summary>
    private List<Attribute> SelectTwoAttributes(Dictionary<Attribute, float> remainingAttr)
    {
        List<Attribute> selectedAttributes = new List<Attribute>();

        for (int i = 0; i < 2; i++)
        {
            if (remainingAttr.Count == 0)
                break;

            float totalWeight = SumOfWeights(remainingAttr);
            if (totalWeight == 0)
            {
                // All weights are zero, so pick randomly
                List<Attribute> keys = new List<Attribute>(remainingAttr.Keys);
                int randIndex = Random.Range(0, keys.Count);
                Attribute chosen = keys[randIndex];
                selectedAttributes.Add(chosen);
                remainingAttr.Remove(chosen);
            }
            else
            {
                float randomNum = Random.Range(0f, totalWeight);
                float cumulativeWeight = 0f;
                Attribute selected = Attribute.Talkative;

                foreach (KeyValuePair<Attribute, float> pair in remainingAttr)
                {
                    cumulativeWeight += pair.Value;
                    if (randomNum <= cumulativeWeight)
                    {
                        selected = pair.Key;
                        break;
                    }
                }
                selectedAttributes.Add(selected);
                remainingAttr.Remove(selected);
            }
        }

        return selectedAttributes;
    }

    private int SelectDestination()
    {
        // Check for available chairs
        for (int i = 0; i < chairs.Count; i++)
        {
            if (!chairOccupied[i])
            {
                // If any chair is available then randomly choose one among unoccupied chairs
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

        return -1; // No available chair found
    }

    private void SpawnCustomer(int chairIndex)
    {
        // Instantiate a new customer and add it to the pool if needed
        GameObject customer = Instantiate(customerPrefab, transform);
        customer.SetActive(false);
        customerPool.Add(customer.GetComponent<CustomerScript>());

        // Generate customer attributes: select two attributes
        Dictionary<Attribute, float> remainingAttr = new Dictionary<Attribute, float>(attributeWeights);
        List<Attribute> attr = SelectTwoAttributes(remainingAttr);

        // Spawn customer
        //GameObject customer = GetPooledCustomer();
        if (customer != null)
        {
            // Optional: log weights for debugging purposes
            Debug.Log(attributeWeights[Attribute.Talkative] + " " + attributeWeights[Attribute.Foodie] + " " + attributeWeights[Attribute.Active]);

            CustomerScript script = customer.GetComponent<CustomerScript>();
            int animNum = Random.Range(1, 5);
            script.num = animNum;

            script.SetAttributes(attr);
            script.SetDestination(chairs[chairIndex]);
            customer.transform.position = entrance.transform.position;
            customer.SetActive(true);
            script.chair = chairIndex;
            AudioManager.Instance.PlayEnterChime(); // Should play enter chime here
        }
        else
        {
            Debug.Log("Failed to get pooled customer");
        }
    }

    public IEnumerator CustomerWave()
    {
        int tempSpawn = numCustomersToSpawn;
        for (int i = 0; i < tempSpawn; i++)
        {
            // Check for an available chair
            int chairIndex = SelectDestination();
            if (chairIndex == -1)
            {
                Debug.Log("No unoccupied chairs available");
                yield break;
            }
            numCustomersToSpawn--;
            SpawnCustomer(chairIndex);
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        }
    }

    private void SendCustomerOut(CustomerScript customer)
    {
        customer.walkout = true;
        chairOccupied[customer.chair] = false;
        customerPool.Remove(customer);
    }

    public void CustomerHours()
    {
        // Iterate backwards to avoid index shifting issues
        for (int i = customerPool.Count - 1; i >= 0; i--)
        {
            CustomerScript customer = customerPool[i];

            if (customer == null)
            {
                customerPool.RemoveAt(i); // Remove destroyed objects
                continue;
            }

            if (customer.gameObject.activeSelf)
            {
                CustomerScript script = customer.GetComponent<CustomerScript>();
                script.hourStayed++;

                // Send customer out after a random duration
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
            if (customer.gameObject.activeSelf)
            {
                SendCustomerOut(customer.GetComponent<CustomerScript>());
            }
        }
    }

    // This overload is not implemented; remove or implement as needed.
    internal void AddCustomerProbability(int v1, double v2, Attribute attribute)
    {
        throw new System.NotImplementedException();
    }
}
