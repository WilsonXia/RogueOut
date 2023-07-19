using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerDirection
{
    None,
    Up,
    Down,
    Left,
    Right,
}

public enum ControllerButton
{
    None,
    A,
    B
}
public class Player : BattleObject
{
    /* Name: Player
     * HP: 15
     * Atk: 3
     * Speed: 5
     */
    public BattleManager battleM;
    // Controller
    ControllerDirection currentDirection;
    ControllerButton currentButton;

    // Properties
    public ControllerDirection ControllerDirection { get { return currentDirection; } }
    public ControllerButton ControllerButton { get { return currentButton; } }

    private void Start()
    {
        name = "Player";
        SetStats(15,3,5);
        SetupSound();
    }
    void Update()
    {
        GetButton();
        GetDirection();
        switch (battleM.State)
        {
            case BattleState.Dialogue:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Advance Dialogue
                }
                break;
            case BattleState.Tutorial:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // End the tutorial
                    battleM.ChangeState(BattleState.Start);
                }
                break;
        }
    }

    void GetButton()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentButton = ControllerButton.A;
        }
        else
        {
            currentButton = ControllerButton.None;
        }
    } // Fix later
    void GetDirection() // Fix later
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentDirection = ControllerDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentDirection = ControllerDirection.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            currentDirection = ControllerDirection.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentDirection = ControllerDirection.Right;
        }
        else
        {
            currentDirection = ControllerDirection.None;
        }
    }
}
