using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public class PlayerManager : MonoBehaviour
{
    public PlayerScript Player;
    private Rigidbody2D rb;

    float speedX, speedY = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = Player.rb;
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        speedX = 0f;
        speedY = 0f;

        if (Input.GetAxis("Horizontal") != 0)
        {
            speedX = Input.GetAxis("Horizontal") * Player.moveSpeed;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            speedY = Input.GetAxis("Vertical") * Player.moveSpeed;
        }

        Debug.Log("speedX" + speedX + ", speedY" + speedY);
        rb.velocity = new Vector2(speedX, speedY).normalized * Player.moveSpeed;

        //flip sprite
        if (speedX > 0)
        {
            Player.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (speedX < 0)
        {
            Player.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
