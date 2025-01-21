using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CatManager : MonoBehaviour
{
    public List<Vector3Int> customSpawnLocations;
    public GameObject[] catPrefabs;
    public Tilemap tilemap;

    public void Start()
    {
        SpawnCats();
    }

    // spawns cats in specific locations on the tilemap
    public void SpawnCats()
    {
        if (customSpawnLocations == null || customSpawnLocations.Count == 0)
        {
            Debug.LogWarning("No spawn locations specified");
        }

        foreach (var cellPosition in customSpawnLocations)
        {
            Vector3 worldPosition = tilemap.CellToWorld(cellPosition);
            foreach (var cat in catPrefabs)
            {
                Instantiate(cat, worldPosition, Quaternion.identity);
            }
        }
    }
}
