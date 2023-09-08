using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState
{
    Tutorial,
    Start,
    Menu,
    Targeting,
    Breakout,
    Dialogue,
    GameOver,
    Victory
}

public class BattleManager : MonoBehaviour
{
    public bool useTutorial;
    [SerializeField]
    BattleState currentState;
    // References
    public GameObject breakoutManager;
    public BattleHUD hud;
    SpeechBox sBox;
    EnemySpawner enemySpawner;
    
    public List<GameObject> enemies;
    GameObject target;
    // Turn Sequence
    [SerializeField]
    Queue<GameObject> turnSequence;
    GameObject actingObject; // The currently acting object in the turn sequence
    int turnNumber; // Tracks how many turns were taken
    float timer;

    // Properties
    public BattleState State { get { return currentState; } }
    public BreakoutManager Breakout { get { return breakoutManager.GetComponent<BreakoutManager>(); } }
    public PlayerController PlayerControl { get { return GameData.instance.PlayerController; } }
    public PlayerData PlayerData { get { return GameData.instance.Player; } }
    public int TurnNumber { get { return turnNumber; } }

    void Start()
    {
        // Connect components
        sBox = hud.Speech;
        // Setup turn sequence
        turnSequence = new Queue<GameObject>();
        turnNumber = 1;
        // Setup Enemies
        enemySpawner = GetComponent<EnemySpawner>();
        if(GameData.instance.Map != null)
        {
            enemySpawner.enemyPrefabs = GameData.instance.Map.currentRoom.GetComponent<EnemyRoom>().enemyPrefabs;
        }
        enemies = enemySpawner.SpawnEnemies();
        target = enemies[0];
        // Defaults
        timer = 0;
        if (useTutorial)
        {
            ChangeState(BattleState.Tutorial);
        }
        else
        {
            ChangeState(BattleState.Start);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case BattleState.Start:
                timer += Time.deltaTime;
                if (timer > 2.5f)
                {
                    timer = 0;
                    SetUpTurnOrder();
                    actingObject = turnSequence.Peek();
                    UseTurn();
                }
                break;
            case BattleState.Dialogue:
                timer += Time.deltaTime;
                if (timer > 2.5f)
                {
                    timer = 0;
                    if (turnSequence.Count <= 0) 
                    {
                        SetUpTurnOrder();
                        actingObject = turnSequence.Peek();
                    }
                    CleanSequence();
                    NextTurn();
                }
                break;
            case BattleState.Breakout:
                foreach (GameObject go in enemies)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.white;
                }
                break;
            case BattleState.Targeting:
                // Show what is being selected
                sBox.SetMessage("Select a target.");
                Color tmp = target.GetComponent<SpriteRenderer>().color;
                tmp.a = 0.7f;
                foreach(GameObject go in enemies)
                {
                    if (go.Equals(target))
                    {
                        go.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        go.GetComponent<SpriteRenderer>().color = tmp;
                    }
                }
                break;
            case BattleState.Victory:
                // Enter Levelup Phase and level up player
                GameData.instance.Player.Attack += 2;
                // Switch scenes 1sec after
                timer += Time.deltaTime;
                if(timer > 1f)
                {
                    GameData.instance.ChangeState(GameState.Map);
                    SceneManager.LoadScene("Map 1");
                }
                break;
            default:
                if (PlayerData.IsDead)
                {
                    ChangeState(BattleState.GameOver);
                }
                else if (enemies.Count == 0)
                {
                    ChangeState(BattleState.Victory);
                }
                break;
        }
    }

    public void ChangeState(BattleState newState)
    {
        // Used to hide graphics, decides player attack
        hud.ShowUI(newState);
        switch (newState)
        {
            case BattleState.Menu:
                Debug.Log(enemies.Count);
                break;
            case BattleState.Breakout:
                Breakout.wallPrefab = target.GetComponent<Enemy>().Wall;
                breakoutManager.SetActive(true);
                Breakout.StartGame();
                break;
            case BattleState.Dialogue:
                breakoutManager.SetActive(false);
                if (currentState == BattleState.Breakout)
                {
                    CalculatePlayerDamage();
                }
                break;
            default:
                breakoutManager.SetActive(false);
                break;
        }
        currentState = newState;
    }

    void CalculatePlayerDamage()
    {
        // This means attack was selected
        // Damage Multiplier (Based on how well you did in Breakout)
        float damage = PlayerData.Attack;

        if (Breakout.BricksLeft >= 7)
        {
            damage = damage * 0.7f;

        }
        else if (Breakout.BricksLeft == 0)
        {
            damage *= 2f;
        }
        int playerDamage = (int)damage;
        sBox.SetMessage(string.Format("You attacked and did {0} damage.", playerDamage));
        Debug.Log(string.Format("You attacked and did {0} damage.", playerDamage));
        PlayerData.PlaySound();
        target.GetComponent<Enemy>().TakeDamage(playerDamage);
    }

    /// <summary>
    /// Orders the combatants into the Turn Queue based on speed.
    /// Use only when Turn Queue is empty.
    /// </summary>
    void SetUpTurnOrder()
    {
        GameObject fastestEnemy = enemies[0];
        int playerSpeed = PlayerData.Speed;
        // Get the fastest Enemy
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().Speed > fastestEnemy.GetComponent<Enemy>().Speed)
            {
                fastestEnemy = enemy;
            }
        }
        int topEnemySpeed = fastestEnemy.GetComponent<Enemy>().Speed;
        // Decide order based on speed
        if (topEnemySpeed < playerSpeed)
        {
            EnqueuePlayer();
        }
        else if (topEnemySpeed > playerSpeed)
        {
            EnqueueEnemy();
        }
        else
        {
            float chance = Random.value;
            if ( chance < 0.5f)
            {
                EnqueuePlayer();
            }
            else
            {
                EnqueueEnemy();
            }
        }
    }
    void CleanSequence()
    {
        // Make a makeshift list to hold the dead enemies
        // In the loop, add enemies that are alive into the new queue
        // Then empty the dead enemies.
        // Also change the target at the end
        List<GameObject> dumpList = new List<GameObject>();
        Queue<GameObject> newSequence = new Queue<GameObject>();
        while (turnSequence.Count > 0)
        {
            BattleObject battler = turnSequence.Dequeue().GetComponent<BattleObject>();
            
            if (!battler.IsDead)
            {
                newSequence.Enqueue(battler.gameObject);
            }
            else
            {
                dumpList.Add(battler.gameObject);
            }
        }
        for (int i = 0; i < dumpList.Count; i++)
        {
            // Remove dead enemies in the dumplist
            for (int j = 0; j < enemies.Count; j++)
            {
                GameObject enemy = enemies[j];
                // Check if they are the same enemy
                if (dumpList[i] == enemy)
                {
                    // Remove that enemy and move to the next
                    enemies.RemoveAt(j);
                    j = enemies.Count;
                }
                if (target == enemy && enemies.Count > 0)
                {
                    target = enemies[0];
                }
            }
            Destroy(dumpList[i]);
        }
        turnSequence = newSequence;
    }
    void EnqueuePlayer()
    {
        // Player goes first
        turnSequence.Enqueue(GameData.instance.Player.gameObject);
        // Followed by enemies
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemy = enemies[i];
            if (!enemy.GetComponent<Enemy>().IsDead)
            {
                turnSequence.Enqueue(enemy);
            }
            else
            {
                enemies.RemoveAt(i);
                Destroy(enemy);
                i--;
            }
        }
    }
    void EnqueueEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemy = enemies[i];
            if (!enemy.GetComponent<Enemy>().IsDead)
            {
                turnSequence.Enqueue(enemy);
            }
            else
            {
                enemies.RemoveAt(i);
                Destroy(enemy);
                i--;
            }
        }
        turnSequence.Enqueue(GameData.instance.Player.gameObject);
    }
    public void NextTurn()
    {
        if (PlayerData.IsDead)
        {
            ChangeState(BattleState.GameOver);
        }
        else if (enemies.Count == 0)
        {
            ChangeState(BattleState.Victory);
        }
        else
        {
            // Receive Next Object
            turnSequence.Dequeue();
            // Check if turn sequence is empty
            if (turnSequence.Count <= 0)
            {
                CleanSequence();
                SetUpTurnOrder();
                turnNumber++;
            }
            actingObject = turnSequence.Peek();
            UseTurn();
        }
    }
    void UseTurn()
    {
        // if Player Turn
        if (actingObject.Equals(GameData.instance.Player.gameObject))
        {
            ChangeState(BattleState.Menu);
        }
        else // if Enemy Turn
        {
            // Enter Dialogue
            ChangeState(BattleState.Dialogue);
            // Enemy Acts
            Enemy e = actingObject.GetComponent<Enemy>();
            e.Act();
            // Show Message
            sBox.SetMessage(string.Format("{0}{1}", e.Name, e.Message));
            Debug.Log(string.Format("{0}{1}", e.Name, e.Message));
            // Respond to enemy's action
            if (e.IsAttacking)
            {
                PlayerData.TakeDamage(e.Attack);
            }
        }
    }
    
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
