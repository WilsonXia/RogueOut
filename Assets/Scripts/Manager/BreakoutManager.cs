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
    public BrkCollisionManager colManager;

    // Fields
    public int lives;
    public bool lifeLost;
    float timer;
    
    public float gameTime;
    float gameTimer;
    bool started = false;

    // Properties
    public Ball p_Ball { get { return ball; } set { ball = value; } }
    public Paddle p_Paddle { get { return paddle; } set { paddle = value; } }
    public Vector2 BoundPoint { get { return boundPoint; } }
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

            // Manager
            colManager.SetUp(this);
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
        timer = 0;
        gameTimer = gameTime;
        DestroyBricks();
        CreateWall();
        ball.gameObject.SetActive(true);
    }
    
    void Update()
    {
        // Only work in Breakout State
        if (battleM.State == BattleState.Breakout)
        {
            if (ball.Launched)
            {
                gameTimer -= Time.deltaTime;
            }
            if (gameTimer < 0)
            {
                gameTimer = gameTime;
                EndGame();
            }
            if (lives > 0 && bricks.Count > 0) // If there are still lives, AND bricks to hit, play
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
                    }
                }
                colManager.BrickCheck(bricks);
                colManager.CleanList(bricks);
            }
            else
            {
                timer += Time.deltaTime;
                if (timer > 0.8f)
                {
                    timer = 0;
                    EndGame();
                }
            }
            colManager.BoundsCheck();
            colManager.PaddleBoundsCheck();
            colManager.PaddleCheck();
        }
    }

    void EndGame()
    {
        // Clear the Game
        ball.Reset();
        ball.gameObject.SetActive(false);
        paddle.Reset();

        battleM.ChangeState(BattleState.Dialogue);
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
