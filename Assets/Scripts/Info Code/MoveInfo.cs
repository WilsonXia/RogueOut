using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MoveInfo : MonoBehaviour
{
    [SerializeField]
    int moveSpeed;
    Vector2 direction = Vector2.zero;
    Vector2 position = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    public Vector2 Position { get { return position; } set { position = value; } }
    public Vector2 Direction { get { return direction; } set { direction = value; } }
    void Start()
    {
        position = transform.position;
    }
    public void Move()
    {
        // Update so that the projectile moves in the direction
        // it is fired in.
        direction = direction.normalized;
        velocity = direction * moveSpeed * Time.deltaTime;
        position += velocity;
        transform.position = position;
    }

    public void Reset()
    {
        direction = Vector2.zero;
        velocity = Vector2.zero;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 direct = new Vector2(transform.position.x + direction.x, transform.position.y + direction.y);
        Gizmos.DrawLine(transform.position, direct);
    }
}
