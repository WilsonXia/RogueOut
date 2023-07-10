using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BattleObject
{
    // References
    public Paddle paddle;
    public Ball ball;
    public BattleManager battleM;

    // Update is called once per frame
    void Update()
    {
        switch (battleM.State)
        {
            case TurnState.Breakout:
                // In Breakout Phase
                if (Input.GetKey(KeyCode.A))
                {
                    paddle.MoveState = Movement.Left;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    paddle.MoveState = Movement.Right;
                }
                else
                {
                    paddle.MoveState = Movement.None;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!ball.Launched)
                    {
                        ball.Launch();
                    }
                }
                break;
           case TurnState.Menu:
                if (Input.GetKeyDown(KeyCode.S))
                {
                    battleM.ChangeAction(1);
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    battleM.ChangeAction(-1);
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Select
                    battleM.ConfirmAction();
                }
                //
                break;
        }
    }
}
