using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CatAnims : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private AIPath ai;
    Vector2 movement;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        movement = ai.desiredVelocity;
        float moveX = movement.x;
        float moveY = movement.y;

        if (moveX == 0 && moveY == 0)
        {
            anim.Play("idle");
        }

        if (moveX > 0)
        {
            anim.Play("rightWalk");
        }
        else if (moveX < 0)
        {
            anim.Play("leftWalk");
        }
        if (moveY > 0 && moveX == 0)
        {
            anim.Play("upWalk");
        }
        else if (moveY < 0 && moveX == 0)
        {
            anim.Play("downWalk");
        }
    }
}
