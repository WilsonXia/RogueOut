using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Start,
    Enemy,
    Event,
    Treasure
}
public class Room : MonoBehaviour
{
    public RoomType type;
    public bool visited;
    public float avoidRadius;
    public Room up;
    public Room down;
    public Room left;
    public Room right;

    public void MoveAbove(Room baseRoom)
    {
        Vector2 basePosition = baseRoom.Position;
        Position = basePosition + Vector2.up * avoidRadius;
    }
    public void MoveBelow(Room baseRoom)
    {
        Vector2 basePosition = baseRoom.Position;
        Position = basePosition + Vector2.down * avoidRadius;
    }
    public void MoveLeft(Room baseRoom)
    {
        Vector2 basePosition = baseRoom.Position;
        Position = basePosition + Vector2.left * avoidRadius;
    }
    public void MoveRight(Room baseRoom)
    {
        Vector2 basePosition = baseRoom.Position;
        Position = basePosition + Vector2.right * avoidRadius;
    }
    public Vector2 Position { get { return transform.position; } set { transform.position = value; } }
}
