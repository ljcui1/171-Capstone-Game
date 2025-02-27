using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingObjectScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -200;
    private float rotationSpeed; // Randomize spin speed // Make rotation speed variable

    public GameObject popAnimation;

    void Start()
    {
        rotationSpeed = Random.Range(-100f, 100f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Vector3.down * moveSpeed * Time.unscaledDeltaTime;
        transform.rotation = transform.rotation * Quaternion.Euler(Vector3.forward * rotationSpeed * Time.unscaledDeltaTime);
        if (transform.position.y < deadZone)
        {
            // Debug.Log("Bean Destroyed");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        // Debug.Log($"BEAN TRIGGERED {obj.gameObject.name}");
        if (obj.gameObject.name == "Ground")
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Instantiate(popAnimation, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
