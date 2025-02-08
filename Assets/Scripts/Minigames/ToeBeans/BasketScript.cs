using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasketScript : MonoBehaviour
{
    public float speed;
    public bool movementOn = true;
    private float minX, maxX;
    public Minigame2Manager manager;

    void Start()
    {
        // Get the camera's left and right edges in world space
        Camera mainCamera = Camera.main;
        float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x; // Half width of the basket

        minX = mainCamera.ScreenToWorldPoint(Vector3.zero).x + halfWidth;
        maxX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - halfWidth;
    }

    void Update()
    {
        if (movementOn)
        {
            MoveBasket();
        }
        movementOn = !manager.gameOver;
    }

    void MoveBasket()
    {
        float moveX = Input.GetAxis("Horizontal");
        float newX = transform.position.x + (moveX * speed * Time.deltaTime);

        // Clamp position to stay within bounds
        newX = Mathf.Clamp(newX, minX, maxX);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"BASKET TRIGGERED {collision.gameObject.name}");
        if (collision.gameObject.tag == "Bean" && movementOn)
        {
            Debug.Log("adding score");
            manager.AddScore(1);
            Destroy(collision.gameObject);
        }
    }
}
