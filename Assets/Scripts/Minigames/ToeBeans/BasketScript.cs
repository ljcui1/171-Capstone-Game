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
    public ToeBeansMinigame manager;

    void Awake()
    {
        // Get the camera's left and right edges in world space
        Camera mainCamera = Camera.main;
        float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x; // Half width of the basket
        manager = FindObjectOfType<ToeBeansMinigame>();

        minX = mainCamera.ScreenToWorldPoint(Vector3.zero).x + halfWidth;
        maxX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - halfWidth;

        Physics2D.IgnoreLayerCollision(0, 9, true);
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
        float moveX = Input.GetAxisRaw("Horizontal");
        // Debug.Log("MOVEX: " + moveX);
        float newX = transform.position.x + (moveX * speed * Time.unscaledDeltaTime);

        // Clamp position to stay within bounds
        newX = Mathf.Clamp(newX, minX, maxX);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bean" && movementOn)
        {
            manager.AddScore(1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Fur" && movementOn)
        {
            manager.AddScore(-1);
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
    }
}
