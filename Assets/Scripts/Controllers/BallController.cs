using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    PlayerController player;
    Ball ball;
    public void SetUp(PlayerController p, Ball b)
    {
        player = p;
        ball = b;
    }
    void Update()
    {
        switch (player.ControllerButton)
        {
            case ControllerButton.A:
                if (!ball.Launched)
                {
                    ball.Launch();
                }
                break;
            default:
                break;
        }
    }
}
