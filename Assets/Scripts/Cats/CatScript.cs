using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;
//using UnityEditor.Experimental.GraphView;
public class CatScript : BaseNPC
{
  private CatManager catManager;
  public float minTime = 8f;
  public float maxTime = 20f;

  public void Start()
  {
    catManager = FindObjectOfType<CatManager>(); // or use your preferred method to get the manager
    StartCoroutine(RandomRepeat());
  }

  private IEnumerator RandomRepeat()
  {
    while (true)
    {
      float delay = Random.Range(minTime, maxTime);
      yield return new WaitForSeconds(delay);
      SetRandomLocation();
    }
  }

  public void SetRandomLocation()
  {
    int randomIndex = Random.Range(0, catManager.locations.Length);
    GameObject randomLocation = catManager.locations[randomIndex];
    SetDestination(randomLocation);
    Debug.Log($"Set {name}'s target to {randomLocation.name}");
  }
}
