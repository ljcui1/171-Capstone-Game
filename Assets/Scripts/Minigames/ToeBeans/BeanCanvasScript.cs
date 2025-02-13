using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanCanvasScript : MonoBehaviour
{
    public float moveSpeed = 300f; // UI elements use higher speed values
    public float deadZone = -500f; // Adjust based on UI canvas size

    private RectTransform beanRect;
    private RectTransform canvasRect;

    void Start()
    {
        beanRect = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move bean down using RectTransform
        beanRect.anchoredPosition += Vector2.down * moveSpeed * Time.unscaledDeltaTime;

        // Destroy bean if it goes below the canvas bounds
        if (beanRect.anchoredPosition.y < deadZone)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.name == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
