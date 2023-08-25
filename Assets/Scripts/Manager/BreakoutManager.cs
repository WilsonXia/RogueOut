using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutManager : MonoBehaviour
{
    // Ball and Paddle
    Ball ball;
    Paddle paddle;
    public GameObject ballObject;
    public GameObject paddleObject;
    // Controllers
    BallController b_controller;
    PaddleController p_controller;
    // Bricks
    List<GameObject> bricks;
    public GameObject brickPrefab;
    public GameObject wallPrefab;
    GameObject wallObj;
    // Others
    public GameObject boundingBox;
    Vector2 boundPoint;
    public BattleManager battleM;

    // Fields
    int lives;
    bool lifeLost;
    int score;
    float timer;
    
    public float gameTime;
    float gameTimer;
    bool started = false;

    // Properties
    public float Timer { get { return gameTimer; } }
    public int BricksLeft { get { return bricks.Count; } }

    private void Start()
    {
        if (!started)
        {
            // Ball and Paddle
            ball = ballObject.GetComponent<Ball>();
            paddle = paddleObject.GetComponent<Paddle>();

            // Controller Setup
            p_controller = GetComponent<PaddleController>();
            p_controller.SetUp(battleM.PlayerControl, paddle);
            b_controller = GetComponent<BallController>();
            b_controller.SetUp(battleM.PlayerControl, ball);
            
            // Bricks and Bounding Box
            bricks = new List<GameObject>();
            boundPoint = boundingBox.GetComponent<SpriteRenderer>().bounds.max;
            started = true;
        }
    }

    public void StartGame()
    {
        if (!started)
        {
            Start();
        }
        // Fields
        lives = 1;
        lifeLost = false;
        score = 0;
        timer = 0;
        gameTimer = gameTime;
        DestroyBricks();
        CreateWall();
    }
    // Update is called once per frame
    void Update()
    {
        if (battleM.State == BattleState.Breakout)
        {
            gameTimer -= Time.deltaTime;
            if(gameTimer < 0) 
            {
                gameTimer = gameTime;
                EndGame();
            }
            if(lives > 0 && bricks.Count > 0) // If there are still lives, AND bricks to hit, play
            {
                // Handle life lost
                if (lifeLost)
                {
                    // Wait for a sec
                    timer += Time.deltaTime;
                    if (timer > 1)
                    {
                        lifeLost = false;
                        timer = 0;
                        ball.Launch();
                    }
                }
                BrickCheck();
                CleanList(bricks);
            }
            else
            {
                timer += Time.deltaTime;
                if (timer > 0.9f)
                {
                    timer = 0;
                    EndGame();
                }
            }
            BoundsCheck();
            PaddleBoundsCheck();
            PaddleCheck();
        }
    }

    void EndGame()
    {
        // Clear the Game
        ball.Reset();
        paddle.Reset();

        battleM.ChangeState(BattleState.Dialogue);
    }

    #region Collisions
    // Check Collisions
    void BoundsCheck()
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
        else if (BottomBoundCheck(ball.ObjectInfo.MinY) && !lifeLost)
        {
            lives--;
            lifeLost = true;
            ball.Reset();
        }
        // Check other projectiles LATER
    }// Fix when other projectiles come
    void PaddleBoundsCheck()
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
    void BrickCheck()
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
                if (brick.GetComponent<ObjectInfo>().IsDead)
                {
                    score++;
                }
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
    public void EmptyList(List<GameObject> list)
    {
        while (list.Count > 0)
        {
            GameObject go = list[0];
            Destroy(go);
            list.RemoveAt(0);
        }
    }
    void DestroyBricks()
    {
        EmptyList(bricks);
        Destroy(wallObj);
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
        Vector2 spawnPoint = new Vector2(boundPoint.x - width / 2, boundPoint.y - height / 2);
        Vector2 pos = new Vector2(Random.Range(-spawnPoint.x, spawnPoint.x), Random.Range(0, spawnPoint.y));
        GameObject newBrick = Instantiate(brickPrefab, pos, Quaternion.identity);
        bricks.Add(newBrick);
    }
    void CreateWall()
    {
        wallObj = Instantiate(wallPrefab, transform);
        foreach (Transform t in wallObj.transform)
        {
            bricks.Add(t.gameObject);
        }
    }
}
