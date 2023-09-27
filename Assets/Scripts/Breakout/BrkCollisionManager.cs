using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class BrkCollisionManager : MonoBehaviour
{
    BreakoutManager breakoutManager;
    Ball ball;
    Paddle paddle;
    Vector2 boundPoint;

    public void SetUp(BreakoutManager br)
    {
        breakoutManager = br;
        ball = br.p_Ball;
        paddle = br.p_Paddle;
        boundPoint = br.BoundPoint;
    }

    #region Collisions
    // Check Collisions
    public void BoundsCheck()
    {
        // Check the ball if it exceeds bounds
        if (LeftBoundCheck(ball.ObjectInfo.MaxX))
        {
            ball.MoveInfo.Position = new Vector2(boundPoint.x - ball.ObjectInfo.Radius, ball.MoveInfo.Position.y);
            if (ball.MoveInfo.Direction.x > 0)
            {
                ball.BounceX();
            }
        }
        else if (RightBoundCheck(ball.ObjectInfo.MinX))
        {
            ball.MoveInfo.Position = new Vector2(-boundPoint.x + ball.ObjectInfo.Radius, ball.MoveInfo.Position.y);
            if (ball.MoveInfo.Direction.x < 0)
            {
                ball.BounceX();
            }
        }
        if (TopBoundCheck(ball.ObjectInfo.MaxY))
        {
            ball.MoveInfo.Position = new Vector2(ball.MoveInfo.Position.x, boundPoint.y - ball.ObjectInfo.Radius);
            if (ball.MoveInfo.Direction.y > 0)
            {
                ball.BounceY();
            }
        }
        else if (BottomBoundCheck(ball.ObjectInfo.MinY)&& !breakoutManager.lifeLost)
        {
            breakoutManager.lives--;
            breakoutManager.lifeLost = true;
            ball.Reset();
        }
        // Check other projectiles LATER
    }// Fix when other projectiles come
    public void PaddleBoundsCheck()
    {
        if (LeftBoundCheck(paddle.ObjectInfo.MaxX))
        {
            paddle.MoveInfo.Position = new Vector2(boundPoint.x - paddle.ObjectInfo.Width / 2, paddle.transform.position.y);
        }
        else if (RightBoundCheck(paddle.ObjectInfo.MinX))
        {
            paddle.MoveInfo.Position = new Vector2(-boundPoint.x + paddle.ObjectInfo.Width / 2, paddle.transform.position.y);
        }
    }
    #region Bound Checks
    private bool LeftBoundCheck(float x)
    {
        return boundPoint.x < x;
    }
    private bool RightBoundCheck(float x)
    {
        return -boundPoint.x > x;
    }
    private bool TopBoundCheck(float y)
    {
        return boundPoint.y < y;
    }
    private bool BottomBoundCheck(float y)
    {
        return -boundPoint.y > y;
    }
    #endregion
    public void BrickCheck(List<GameObject> bricks)
    {
        foreach (GameObject br in bricks)
        {
            // if a collision is detected
            if (AABBCollision(br, ball.gameObject))
            {
                float deltaX, deltaY;
                SpriteRenderer brickSprite = br.GetComponent<SpriteRenderer>();
                // Brick Response
                Brick brick = br.GetComponent<Brick>();
                brick.OnHit();
                // Ball Response
                // Obtain deltaX and deltaY
                if (ball.MoveInfo.Direction.x > 0)
                {
                    deltaX = brickSprite.bounds.min.x - ball.Right;
                }
                else
                {
                    deltaX = ball.Right - brickSprite.bounds.max.x;
                }
                if (ball.MoveInfo.Direction.y > 0)
                {
                    deltaY = brickSprite.bounds.min.y - ball.Top;
                }
                else
                {
                    deltaY = brickSprite.bounds.min.y - ball.Bottom;
                }
                // Compare values
                if (deltaX > deltaY)
                {
                    ball.BounceX();
                }
                else if (deltaX < deltaY)
                {
                    ball.BounceY();
                }
                else
                {
                    ball.BounceX();
                    ball.BounceY();
                }
                return;
            }
        }
    }
    public void PaddleCheck()
    {
        if (AABBCollision(ball.ObjectInfo, paddle.ObjectInfo))
        {
            // Count the Bounce Count
            if (ball.MoveInfo.Direction.y < 0) // Check if going down
            {
                Vector2 ballPos = ball.transform.position;
                Vector2 paddlePos = paddle.transform.position;
                ball.Bounce(ballPos - paddlePos);
            }
        }
    }
    // Clean Dead Objects
    public void CleanList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = list[i];
            if (go.GetComponent<ObjectInfo>().IsDead)
            {
                Destroy(go);
                list.RemoveAt(i);
                i--;
            }
        }
    }

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

    #endregion
}
