using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CatManager : MonoBehaviour
{
    public List<LocationPrefabMapping> spawnMapping;
    public readonly List<GameObject> cats = new List<GameObject>(); // Initialize list
    public Tilemap tilemap;
    public GameObject[] locations;

    public static CatManager instance { get; private set; }

    public void Awake()
    {
        // make instance
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

    public void Start()
    {
        // Find all GameObjects with the "Location" tag and add them to the list
        locations = GameObject.FindGameObjectsWithTag("Location");
        Debug.Log($"******* Found {locations.Length} locations ********");
        SpawnCats();

        SaveScript.LoadCatData();

        InvokeRepeating(nameof(SetTargetLocations), 10, 10);
    }

    public void OnApplicationQuit()
    {
        SaveScript.SaveCatData();
    }

    // Spawns cats in specific locations on the tilemap
    public void SpawnCats()
    {
        foreach (var entry in spawnMapping)
        {
            GameObject spawnLocation = entry.spawnLocation;
            GameObject catPrefab = entry.catPrefab;

            if (spawnLocation == null || catPrefab == null)
            {
                Debug.LogWarning("Spawn location or cat prefab is null, skipping entry.");
                continue;
            }

            // Convert cell position to world position
            Vector3Int cellPosition = tilemap.WorldToCell(spawnLocation.transform.position);
            Vector3 worldPosition = tilemap.CellToWorld(cellPosition);

            if (IsPrefab(catPrefab))
            {
                // Instantiate the prefab and add to the list
                GameObject cat = Instantiate(catPrefab, worldPosition, Quaternion.identity);
                cats.Add(cat);
            }
            else
            {
                // If not a prefab, just set its position
                catPrefab.transform.position = worldPosition;
                cats.Add(catPrefab);
            }

            Debug.Log($"Spawned {catPrefab.name} at {worldPosition}");
        }

        SetTargetLocations();
    }

    // Check if a GameObject is a prefab (not instantiated in the scene)
    private bool IsPrefab(GameObject obj)
    {
        return obj.scene.rootCount == 0; // Prefabs are not part of any scene
    }

    // Will be called in GameManager during the day
    private void SetTargetLocations()
    {
        if (locations == null || locations.Length == 0)
        {
            Debug.LogWarning("No locations available to set targets.");
            return;
        }

        // Shuffle the locations to ensure randomness
        List<GameObject> shuffledLocations = new List<GameObject>(locations);
        for (int i = 0; i < shuffledLocations.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledLocations.Count);
            (shuffledLocations[i], shuffledLocations[randomIndex]) = (shuffledLocations[randomIndex], shuffledLocations[i]);
        }

        // Assign each cat a unique location; if not enough locations, assign a random one
        for (int i = 0; i < cats.Count; i++)
        {
            CatScript catScript = cats[i].GetComponent<CatScript>();
            if (catScript == null)
            {
                Debug.LogWarning($"No CatScript found on {cats[i].name}, skipping.");
                continue;
            }

            if (catScript.matched)
            {
                continue;
            }
            Debug.Log("choosing new destination for " + i);
            if (i < shuffledLocations.Count)
            {
                GameObject uniqueLocation = shuffledLocations[i];
                catScript.SetDestination(uniqueLocation);
                // Debug.Log($"Set {cats[i].name}'s target to {uniqueLocation.name}");
            }
            else
            {
                catScript.SetRandomLocation();
            }
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
