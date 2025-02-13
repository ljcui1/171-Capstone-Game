using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanSpawner : MonoBehaviour
{
    public GameObject bean;
    public GameObject fur;
    public float spawnRate = 2f;
    public float widthOffset = 2f;

    [SerializeField] private float timer = 0;

    void Start()
    {
        SpawnBean();
    }

    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.unscaledDeltaTime;
        }
        else
        {
            SpawnBean();
            timer = 0;
        }
    }

    void SpawnBean()
    {
        float leftPoint = transform.position.x - widthOffset;
        float rightPoint = transform.position.x + widthOffset;
        Instantiate(bean, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), transform.rotation);
    }

}
