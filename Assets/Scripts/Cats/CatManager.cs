using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CatManager : MonoBehaviour
{
    [SerializeField] private List<LocationPrefabMapping> spawnMapping;
    public Tilemap tilemap;

    public void Start()
    {
        SpawnCats();
    }

    // spawns cats in specific locations on the tilemap
    public void SpawnCats()
    {
        foreach (var entry in spawnMapping)
        {
            Vector3Int spawnLocation = entry.spawnLocation;
            GameObject catPrefab = entry.catPrefab;

            // Convert cell position to world position
            Vector3 worldPosition = tilemap.CellToWorld(spawnLocation);

            // Instantiate the specific cat prefab at the spawn location
            Instantiate(catPrefab, worldPosition, Quaternion.identity);

            Debug.Log($"Spawned {catPrefab.name} at {worldPosition}");
        }
    }
}

// Serializable class to hold the key-value pair for spawn location and prefab
[System.Serializable]
public class LocationPrefabMapping
{
    public Vector3Int spawnLocation; // Tilemap cell position
    public GameObject catPrefab; // Corresponding cat prefab
}
