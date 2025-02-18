using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject fallingObj;
    public float spawnRate = 2f;
    public float widthOffset = 2f;

    [SerializeField] private float timer = 0;

    void Start()
    {
        SpawnObject();
    }

    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.unscaledDeltaTime;
        }
        else
        {
            SpawnObject();
            timer = 0;
        }
    }

    void SpawnObject()
    {
        float leftPoint = transform.position.x - widthOffset;
        float rightPoint = transform.position.x + widthOffset;
        Instantiate(fallingObj, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), transform.rotation);
    }

}
