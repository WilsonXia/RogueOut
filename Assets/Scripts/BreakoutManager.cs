using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Ball ball;
    public Paddle paddle;
    public Brick brickPrefab;
    public GameObject wallPrefab;
    public GameObject boundingBox;
    Vector2 boundPoint;

    int lives;
    bool lifeLost;
    float timer;

    [SerializeField]
    List<GameObject> bricks;

    
    private void Start()
    {
        lives = 3;
        bricks = new List<GameObject>();
        boundPoint = boundingBox.GetComponent<SpriteRenderer>().bounds.max;
    }
    // Update is called once per frame
    void Update()
    {
        if(lives > 0)
        {
            if (bricks.Count == 0)
            {
                CreateWall();
                //CreateBrick();
            }
            if (lifeLost)
            {
                timer += Time.deltaTime;
                if (timer > 1)
                {
                    lifeLost = false;
                    timer = 0;
                    ball.Launch();
                }
            }
            BoundsCheck();
            CheckBricks();
            CheckPaddle();
            CleanList(bricks);
        }
    }
    #region Collisions
    // Check Collisions
    public void BoundsCheck()
    {
        // Check if paddle exceeds bounds (edit later?)
        if (LeftBoundCheck(paddle.ObjectInfo.MaxX))
        {
            paddle.MoveInfo.Position = new Vector2(boundPoint.x - paddle.ObjectInfo.Width/2, paddle.transform.position.y);
        }
        else if (RightBoundCheck(paddle.ObjectInfo.MinX))
        {
            paddle.MoveInfo.Position = new Vector2(-boundPoint.x + paddle.ObjectInfo.Width/2, paddle.transform.position.y);
        }
        // Check the ball if it exceeds bounds
        if(LeftBoundCheck(ball.ObjectInfo.MaxX)) 
        {
            ball.MoveInfo.Position = new Vector2(boundPoint.x - ball.ObjectInfo.Radius, ball.MoveInfo.Position.y);
            ball.BounceX();
        }
        else if(RightBoundCheck(ball.ObjectInfo.MinX))
        {
            ball.MoveInfo.Position = new Vector2(-boundPoint.x + ball.ObjectInfo.Radius, ball.MoveInfo.Position.y);
            ball.BounceX();
        }
        if(TopBoundCheck(ball.ObjectInfo.MaxY))
        {
            ball.MoveInfo.Position = new Vector2(ball.MoveInfo.Position.x, boundPoint.y - ball.ObjectInfo.Radius);
            ball.BounceY();
        }
        else if (BottomBoundCheck(ball.ObjectInfo.MinY))
        {
            lives--;
            lifeLost = true;
            ball.MoveInfo.Reset();
        }
        // Check other projectiles LATER
    }// Fix when other projectiles come

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
    public void CheckBricks()
    {
        foreach(GameObject br in bricks)
        {
            // if a collision is detected
            if(AABBCollision(br, ball.gameObject))
            {
                // Register brick hit
                br.GetComponent<Brick>().OnHit();
                // Decide how to bounce ball
                // Obtain deltaX and deltaY
                // Compare values
                Vector2 toBall = ball.transform.position - br.transform.position;
                if(Mathf.Abs(toBall.x) > Mathf.Abs(toBall.y))
                {
                    ball.BounceX();
                }
                else if (Mathf.Abs(toBall.x) < Mathf.Abs(toBall.y))
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
    public void CheckPaddle()
    {
        if (AABBCollision(ball.ObjectInfo, paddle.ObjectInfo)) 
        {
            // Count the Bounce Count
            if(ball.MoveInfo.Direction.y < 0) // Check if going down
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
        for(int i = 0; i < list.Count; i++)
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

    void CreateBrick() 
    {
        // Edit to match grid later
        float width = 2;
        float height = 1;
        Vector2 spawnPoint = new Vector2(boundPoint.x - width/2, boundPoint.y - height/2);
        Vector2 pos = new Vector2(Random.Range(-spawnPoint.x, spawnPoint.x), Random.Range(0, spawnPoint.y));
        GameObject newBrick = Instantiate(brickPrefab, pos, Quaternion.identity).gameObject;
        bricks.Add(newBrick);
    }
    void CreateWall()
    {
        GameObject wall = Instantiate(wallPrefab);
        foreach (Transform t in wall.transform)
        {
            bricks.Add(t.gameObject);
        }
    }
}
