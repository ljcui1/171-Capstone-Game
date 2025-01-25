using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CatManager : MonoBehaviour
{
    [SerializeField] private List<LocationPrefabMapping> spawnMapping;

    [SerializeField] private List<GameObject> cats;
    public Tilemap tilemap;
    public GameObject[] locations;

    public void Start()
    {
        // Find all GameObjects with the "Location" tag and add them to the list
        locations = GameObject.FindGameObjectsWithTag("Location");
        Debug.Log($"******* {locations} ********");
        SpawnCats();
    }

    // spawns cats in specific locations on the tilemap
    public void SpawnCats()
    {
        foreach (var entry in spawnMapping)
        {
            GameObject spawnLocation = entry.spawnLocation;
            GameObject catPrefab = entry.catPrefab;

            // Convert cell position to world position
            Vector3Int cellPosition = tilemap.WorldToCell(spawnLocation.transform.position);
            Vector3 worldPosition = tilemap.CellToWorld(cellPosition);

            // Instantiate the specific cat prefab at the spawn location
            GameObject cat = Instantiate(catPrefab, worldPosition, Quaternion.identity);
            cats.Add(cat);

            Debug.Log($"Spawned {catPrefab.name} at {worldPosition}");
        }
        SetTargetLocations();
    }

    // will be called in GameManager during the day
    private void SetTargetLocations()
    {
        // Shuffle the locations to ensure randomness
        List<GameObject> shuffledLocations = new List<GameObject>(locations);
        for (int i = 0; i < shuffledLocations.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledLocations.Count);
            (shuffledLocations[i], shuffledLocations[randomIndex]) = (shuffledLocations[randomIndex], shuffledLocations[i]);
        }

        // Assign each cat a unique location, else if there aren't enough locations, assign a random location
        for (int i = 0; i < cats.Count; i++)
        {
            CatScript catScript = cats[i].GetComponent<CatScript>();
            if (i < shuffledLocations.Count)
            {
                GameObject uniqueLocation = shuffledLocations[i];
                catScript.SetDestination(uniqueLocation);
                Debug.Log($"Set {cats[i].name}'s target to {uniqueLocation.name}");
            }
            else
            {
                SetRandomLocation(cats[i]);
            }
        }
    }

    private void SetRandomLocation(GameObject cat)
    {
        CatScript catScript = cat.GetComponent<CatScript>();
        if (catScript != null && locations.Length > 0)
        {
            int randomIndex = Random.Range(0, locations.Length);
            GameObject randomLocation = locations[randomIndex];
            Debug.Log(randomLocation);
            catScript.SetDestination(randomLocation);
            Debug.Log($"Set {cat.name}'s target to {randomLocation.name}");
        }
        else
        {
            Debug.LogWarning($"CatScript not found on {cat.name} or locations array is empty!");
        }
    }
}

// Serializable class to hold the key-value pair for spawn location and prefab
[System.Serializable]
public class LocationPrefabMapping
{
    public GameObject spawnLocation; // Tilemap cell position
    public GameObject catPrefab; // Corresponding cat prefab
}
