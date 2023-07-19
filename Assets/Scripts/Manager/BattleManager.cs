using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Tutorial,
    Start,
    Menu,
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
    public GameObject player;
    public GameObject breakoutManager;
    public BattleHUD hud;
    SpeechBox sBox;

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
    public Player PlayerOne { get { return player.GetComponent<Player>(); } }
    public int TurnNumber { get { return turnNumber; } }

    void Start()
    {
        // Connect components
        PlayerOne.battleM = this;
        sBox = hud.Speech;
        // Setup turn sequence
        turnSequence = new Queue<GameObject>();
        turnNumber = 1;
        // Setup Target
        if (enemies.Count == 1)
        {
            target = enemies[0];
        }
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
                    CheckTurn();
                }
                break;
            case BattleState.Dialogue:
                timer += Time.deltaTime;
                if (timer > 2.5f)
                {
                    timer = 0;
                    if(turnSequence.Count <= 0) 
                    {
                        SetUpTurnOrder();
                        actingObject = turnSequence.Peek();
                    }
                    CleanSequence();
                    NextTurn();
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Orders the combatants into the Turn Queue based on speed.
    /// Use only when Turn Queue is empty.
    /// </summary>
    void SetUpTurnOrder()
    {
        GameObject fastestEnemy = enemies[0];
        int playerSpeed = PlayerOne.Speed;
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
        Debug.Log(string.Format("Enemy Spd: {0}  Player Spd: {1}", topEnemySpeed, playerSpeed));
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
            Debug.Log(string.Format("Chance: {0}, < 0.5f? {1}", chance, chance < 0.5f));
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
                // Check if they are the same enemy
                if (dumpList[i] == enemies[j])
                {
                    // Remove it
                    enemies.RemoveAt(j);
                    j = enemies.Count;
                }
            }
            Destroy(dumpList[i]);
        }
        turnSequence = newSequence;
    }
    void EnqueuePlayer()
    {
        Debug.Log(string.Format("Player first"));
        // Player goes first
        turnSequence.Enqueue(player);
        // Followed by enemies
        foreach (GameObject enemy in enemies)
        {
            turnSequence.Enqueue(enemy);
        }
    }
    void EnqueueEnemy()
    {
        Debug.Log(string.Format("Enemy first"));
        int playerSpeed = PlayerOne.Speed;
        // Enemies that are faster go first
        foreach (GameObject enemy in enemies)
        {
            int enemySpeed = enemy.GetComponent<Enemy>().Speed;
            // If player is not in queue, AND player is faster than that enemy
            if (!turnSequence.Contains(player) && enemySpeed < playerSpeed)
            {
                // Add the player into the queue
                turnSequence.Enqueue(player);
            }
            // Adds all enemies after execution
            turnSequence.Enqueue(enemy);
        }
    }

    public void NextTurn()
    {
        if (PlayerOne.IsDead)
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
                SetUpTurnOrder();
                turnNumber++;
            }
            actingObject = turnSequence.Peek();
            CheckTurn();
        }
    }
    void CheckTurn()
    {
        // if Player Turn
        if (actingObject.Equals(player))
        {
            //if (currentState == BattleState.Dialogue)
            //{
                ChangeState(BattleState.Menu);
            //}
        }
        else // if Enemy Turn
        {
            // Enter Dialogue
            ChangeState(BattleState.Dialogue);
            // Enemy Acts
            Enemy e = actingObject.GetComponent<Enemy>();
            e.Act();
            e.PlaySound();
            // Show Message
            sBox.SetMessage(string.Format("{0}{1}", e.Name, e.Message));
            // Respond to enemy's action
            if (e.IsAttacking)
            {
                PlayerOne.TakeDamage(e.Attack);
            }
        }
    }

    public void ChangeState(BattleState newState)
    {
        // Used to hide graphics, decides player attack
        hud.ShowUI(newState);
        switch (newState)
        {
            case BattleState.Breakout:
                breakoutManager.SetActive(true);
                Breakout.StartGame();
                break;
            case BattleState.Dialogue:
                breakoutManager.SetActive(false);
                if (currentState == BattleState.Breakout)
                {
                    // This means attack was selected
                    // Damage Multiplier (Based on how well you did in Breakout)
                    float damage = PlayerOne.Attack;

                    if (Breakout.BricksLeft >= 7)
                    {
                        damage = damage * 0.4f;

                    }
                    else if (Breakout.BricksLeft >= 4)
                    {
                        damage = damage * 0.7f;

                    }
                    int playerDamage = (int)damage;
                    sBox.SetMessage(string.Format("You attacked and did {0} damage.", playerDamage));
                    PlayerOne.PlaySound();
                    target.GetComponent<Enemy>().TakeDamage(playerDamage);
                }
                break;
            default:
                breakoutManager.SetActive(false);
                break;
        }
        currentState = newState;
    }

    
}
