using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Movement
{
    None,
    Left,
    Right
}
public class Paddle : Projectile
{
    Movement move_state;
    // Properties
    public Movement MoveState { get { return move_state; } set { move_state = value; } }
    void Start()
    {
        GetComponents();
        move_state = Movement.None;
        bounceCount = 99;
        lifeTime = 99f;
    }

    public void Reset()
    {
        moveInfo.Reset();
        moveInfo.Position = new Vector2(0,-4);
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
        moveInfo.Move();
    }
}
