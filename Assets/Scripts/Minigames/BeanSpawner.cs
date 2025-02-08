using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanSpawner : MonoBehaviour
{
    public GameObject bean;
    public GameObject fur;
    public float spawnRate = 2;
    public float widthOffset = 2;

    [SerializeField] private float timer = 0;

    void Start()
    {
        spawnBean();
    }

    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnBean();
            timer = 0;
        }
    }

    void spawnBean()
    {
        float leftPoint = transform.position.x - widthOffset;
        float rightPoint = transform.position.x + widthOffset;
        Instantiate(bean, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), transform.rotation);
    }

}
