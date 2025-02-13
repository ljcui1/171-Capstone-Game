using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanSpawnerCanvasScript : MonoBehaviour
{
    public GameObject beanPrefab;
    public float spawnRate = 2f;
    private float timer = 0f;

    private RectTransform canvasRect;
    private float minX, maxX;

    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Calculate left & right spawn bounds inside the UI canvas
        float halfCanvasWidth = canvasRect.rect.width * 0.5f;
        float beanWidth = beanPrefab.GetComponent<RectTransform>().rect.width * 0.5f;

        minX = -halfCanvasWidth + beanWidth;
        maxX = halfCanvasWidth - beanWidth;

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
        // Generate a random X position within the UI bounds
        float spawnX = Random.Range(minX, maxX);
        float spawnY = canvasRect.rect.height * 0.5f; // Spawn at the top of the UI canvas

        // Instantiate bean as a UI element
        GameObject newBean = Instantiate(beanPrefab, transform);
        RectTransform beanRect = newBean.GetComponent<RectTransform>();
        beanRect.anchoredPosition = new Vector2(spawnX, spawnY);
    }
}
