using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    Player player;
    Paddle paddle;
    public void SetUp(Player p, Paddle pad)
    {
        player = p;
        paddle = pad;
    }
    void Update()
    {
        switch (player.ControllerDirection)
        {
            case ControllerDirection.Left:
                paddle.MoveState = Movement.Left;
                break;
            case ControllerDirection.Right:
                paddle.MoveState = Movement.Right;
                break;
            default:
                paddle.MoveState = Movement.None;
                break;
        }
    }
}
