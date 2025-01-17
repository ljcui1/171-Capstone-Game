using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public class PlayerManager : MonoBehaviour
{
    public PlayerScript Player;
    private Rigidbody2D rb;

    //float speedX, speedY = 0;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = Player.rb;
    }

    // Update is called once per frame
    void Update()
    {

        /*Debug.Log("Vertical" + Input.GetAxis("Vertical"));
        // movement
        speedX = 0f;
        speedY = 0f;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            speedX = Input.GetAxisRaw("Horizontal") * Player.moveSpeed;
        }
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            speedY = Input.GetAxisRaw("Vertical") * Player.moveSpeed;
        }

        Debug.Log("speedX" + speedX + ", speedY" + speedY);
        rb.velocity = new Vector2(speedX, speedY).normalized * Player.moveSpeed;
        */

        //movement 2
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        //flip sprite
        if (moveInput.x > 0)
        {
            Player.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveInput.x < 0)
        {
            Player.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * Player.moveSpeed;
    }
}
