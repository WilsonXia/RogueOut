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
public class PlayerController : MonoBehaviour
{
    // Controller
    ControllerDirection currentDirection;
    ControllerDirection prevDirection;
    ControllerButton currentButton;

    // Properties
    public ControllerDirection PreviousDirection { get { return prevDirection; } }
    public ControllerDirection ControllerDirection { get { return currentDirection; } }
    public ControllerButton ControllerButton { get { return currentButton; } }

    void Update()
    {
        GetButton();
        GetDirection();
    }

    void GetButton()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentButton = ControllerButton.A;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentButton = ControllerButton.B;
        }
        else
        {
            currentButton = ControllerButton.None;
        }
    } // Fix later
    void GetDirection() // Fix later
    {
        prevDirection = currentDirection;
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
    /// <summary>
    /// Returns whether the previous direction is not equal to the current direction.
    /// In other words, if it was pressed.
    /// </summary>
    /// <returns></returns>
    public bool Pressed()
    {
        return prevDirection != currentDirection;
    }
}
