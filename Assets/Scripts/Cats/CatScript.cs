using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

//using UnityEditor.Experimental.GraphView;

// Saving serialization referenced from https://youtu.be/47QIUHDEaSY?si=M2AFwp2JimxTKGVj
[System.Serializable]
public class CatValues
{
	public List<AttributePair> attributes; // Assuming AttributePair is serializable

	public bool matched;

	public CatValues(CatScript cat)
	{
		matched = cat.matched;
		attributes = new List<AttributePair>(cat.attributes);
	}
}

public class CatScript : BaseNPC
{
	private CatManager catManager;
	public float minTime = 8f;
	public float maxTime = 20f;
	public bool matched = false;

	public GameObject destination;

	public void Start()
	{
		catManager = FindObjectOfType<CatManager>(); // or use your preferred method to get the manager
		StartCoroutine(RandomRepeat());
	}

	private IEnumerator RandomRepeat()
	{
		while (!matched)
		{
			float delay = Random.Range(minTime, maxTime);
			yield return new WaitForSeconds(delay);
			SetRandomLocation();
		}
	}


	public void SetAttributePairs(List<AttributePair> attr)
	{
		attributes.Clear();
		foreach (var attribute in attr)
		{
			attributes.Add(new AttributePair { attribute = attribute.attribute, isActive = attribute.isActive });
		}
	}

	public void SetRandomLocation()
	{
		int randomIndex = Random.Range(0, catManager.locations.Length);
		destination = catManager.locations[randomIndex];
		SetDestination(destination);
		// Debug.Log($"Set {name}'s target to {randomLocation.name}");
	}
}
