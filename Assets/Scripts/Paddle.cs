using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Movement
{
    None,
    Left,
    Right
}
public class Paddle : Projectile
{
    Movement move_state;
    void Start()
    {
        GetComponents();
        move_state = Movement.None;
        bounceCount = 99;
        lifeTime = 99f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (move_state)
        {
            case Movement.None:
                moveInfo.Direction = Vector2.zero;
                break;
            case Movement.Left:
                moveInfo.Direction = Vector2.left;
                break;
            case Movement.Right:
                moveInfo.Direction = Vector2.right;
                break;
        }

        if (Input.GetKey(KeyCode.A))
        {
            move_state = Movement.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move_state = Movement.Right;
        }
        else
        {
            move_state = Movement.None;
        }

        moveInfo.Move();
    }
}
