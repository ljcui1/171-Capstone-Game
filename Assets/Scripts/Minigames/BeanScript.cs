using System.Collections;
using System.Collections.Generic;
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
}
