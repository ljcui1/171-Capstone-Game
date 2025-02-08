using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeanScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -200;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Vector3.down * moveSpeed * Time.deltaTime;

        if (transform.position.y < deadZone)
        {
            Debug.Log("Bean Destroyed");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        // Debug.Log($"BEAN TRIGGERED {obj.gameObject.name}");
        if (obj.gameObject.name == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
