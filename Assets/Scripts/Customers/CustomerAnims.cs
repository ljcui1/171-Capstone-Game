using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CustomerAnims : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private AIPath ai;
    Vector2 movement;

    public int num;

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
            anim.Play("idle" + num);
        }

        if (moveX > 0)
        {
            anim.Play("right" + num);
        }
        else if (moveX < 0)
        {
            anim.Play("left" + num);
        }
    }

}
