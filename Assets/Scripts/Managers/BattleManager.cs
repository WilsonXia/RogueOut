using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState
{
    Menu,
    Breakout,
    Dialougue
}

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    TurnState currentState;
    public GameObject player;
    public GameObject breakoutManager;
    public GameObject actionSelector;
    List<GameObject> enemies;

    Queue<GameObject> turnSequence;
    int turnNumber; // Tracks how many turns were taken

    //bool playerActed; // Holds if the player acted
    //bool breakoutOn;
    
    
    // Properties
    public TurnState State { get { return currentState; } }
    
    //public bool PlayerActed { get { return playerActed; } }

    //public bool BreakoutOn { get { return breakoutOn; } }

    void Start()
    {
        // Decide currentState
        StartState();
        turnNumber = 1;
        player.GetComponent<Player>().battleM = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Check whether in game or in menu
        switch (currentState)
        {
            case TurnState.Menu:
                break;
            case TurnState.Breakout:
                break;
        }
    }

    void StartState()
    {
        // if enemy speed > player speed
        currentState = TurnState.Menu;
    }

    public void ChangeState(TurnState newState) 
    {
        switch (newState)
        {
            case TurnState.Menu:
                breakoutManager.SetActive(false);
                actionSelector.SetActive(true);
                break;
            case TurnState.Breakout:
                breakoutManager.SetActive(true);
                actionSelector.SetActive(false);
                break;
        }
        currentState = newState;
    }

    public void ChangeAction(int step)
    {
        actionSelector.GetComponent<ActionSelector>().ChangeAction(step);
    }

    public void ConfirmAction()
    {
        actionSelector.GetComponent<ActionSelector>().ConfirmAction();
    }
}
