using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Ball ball;
    public Paddle paddle;
    public GameObject boundingBox;
    Vector2 boundPoint;

    // List of Bricks
    private void Start()
    {
        boundPoint = boundingBox.GetComponent<SpriteRenderer>().bounds.max;
    }
    // Update is called once per frame
    void Update()
    {
        BoundsCheck();
        CheckPaddle();
    }

    // Check Collisions
    public void BoundsCheck()
    {
        // Check the ball
        // Exceeds bounds
        if(boundPoint.x < ball.ObjectInfo.MaxX) 
        {
            ball.MoveInfo.Position = new Vector2(boundPoint.x - ball.ObjectInfo.Radius, ball.MoveInfo.Position.y);
            ball.BounceX();
        }
        else if(-boundPoint.x > ball.ObjectInfo.MinX)
        {
            ball.MoveInfo.Position = new Vector2(-boundPoint.x + ball.ObjectInfo.Radius, ball.MoveInfo.Position.y);
            ball.BounceX();
        }
        if(boundPoint.y < ball.ObjectInfo.MaxY)
        {
            ball.MoveInfo.Position = new Vector2(ball.MoveInfo.Position.x, boundPoint.y - ball.ObjectInfo.Radius);
            ball.BounceY();
        }
        else if (-boundPoint.y > ball.ObjectInfo.MinY)
        {
            ball.MoveInfo.Position = new Vector2(ball.MoveInfo.Position.x, -boundPoint.y + ball.ObjectInfo.Radius);
            ball.BounceY();
        }
        // Check other objects
    }
    public void CheckPaddle()
    {
        if (AABBCollision(ball.ObjectInfo, paddle.ObjectInfo)) 
        {
            // Count the Bounce Count
            if(ball.MoveInfo.Direction.y < 0)
            {
                ball.BounceY();
            }
        }
    }
    // Clean Dead Objects

    // AABB Collision
    bool AABBCollision(GameObject a, GameObject b)
    {
        return AABBCollision(a.GetComponent<ObjectInfo>(), b.GetComponent<ObjectInfo>());
    }
    bool AABBCollision(ObjectInfo a, ObjectInfo b)
    {
        bool verdict = a.MinX < b.MaxX && a.MaxX > b.MinX
            && a.MinY < b.MaxY && a.MaxY > b.MinY;
        return verdict;
    }
    // Circle Collision
    //bool CircleCollision(GameObject a, GameObject b) 
    //{
    //    float sqrDistance = Vector3.SqrMagnitude(a.transform.position - b.transform.position);
    //    return CircleCollision(a.GetComponent<ObjectInfo>(), b.GetComponent<ObjectInfo>(), sqrDistance);
    //}

    //bool CircleCollision(ObjectInfo a, ObjectInfo b, float dist)
    //{
    //    return dist < Mathf.Pow(a.Radius + b.Radius, 2);
    //}
}
