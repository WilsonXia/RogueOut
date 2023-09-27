using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Action
{
    Attack,
    Skill,
    Item
}

public class ActionSelector : MonoBehaviour
{
    // Fields
    public GameObject indicator;
    public GameObject atkText, skillText, itemText;
    public BattleManager battleM;

    ActionSelectorController asController;
    AudioSource selectingSound;

    Action currentAction;
    int actionIndex;

    GameObject currentTarget;
    int targetIndex;

    RectTransform atkTransform, skillTransform, itemTransform, iTransform;

    public BattleManager BattleManager { get { return battleM; } }

    // Start is called before the first frame update
    void Start()
    {
        selectingSound = GetComponent<AudioSource>();
        atkTransform = atkText.GetComponent<RectTransform>();
        skillTransform = skillText.GetComponent<RectTransform>();
        itemTransform = itemText.GetComponent<RectTransform>();
        iTransform = indicator.GetComponent<RectTransform>();

        asController = GetComponent<ActionSelectorController>();
        asController.SetUp(battleM.PlayerControl, this);
        actionIndex = 0;

        targetIndex = 0;
    }
    public void ChangeTarget(int step)
    {
        targetIndex += step;
        if (targetIndex < 0)
        {
            targetIndex = battleM.enemies.Count - 1;
        }
        else if (targetIndex > battleM.enemies.Count - 1)
        {
            targetIndex = 0;
        }
        currentTarget = battleM.enemies[targetIndex];
        // Set-up target for the manager
        battleM.SetTarget(targetIndex);
        selectingSound.Play();
    }
    public void SelectTarget()
    {
        // Trigger a game of Breakout
        battleM.ChangeState(BattleState.Breakout);
    }
    public void ChangeAction(int step)
    {
        actionIndex += step;
        if (actionIndex < 0)
        {
            actionIndex = 2;
        }
        else if (actionIndex > 2)
        {
            actionIndex = 0;
        }
        RectTransform selectedAction = new RectTransform();
        switch (actionIndex)
        {
            case 0:
                currentAction = Action.Attack;
                selectedAction = atkTransform;
                break;
            case 1:
                currentAction = Action.Skill;
                selectedAction = skillTransform;
                break;
            case 2:
                currentAction = Action.Item;
                selectedAction = itemTransform;
                break;
        }

        iTransform.anchorMin = new Vector2(iTransform.anchorMin.x, selectedAction.anchorMin.y);
        iTransform.anchorMax = new Vector2(iTransform.anchorMax.x, selectedAction.anchorMax.y);
        selectingSound.Play();
    }

    public void SelectAction()
    {
        selectingSound.Play();
        switch (currentAction)
        {
            case Action.Attack:
                // Check if there are multiple enemies
                if(battleM.enemies.Count > 1)
                {
                    battleM.SetTarget(0);
                    // Find a Target
                    battleM.ChangeState(BattleState.Targeting);
                }
                else
                {
                    battleM.SetTarget(0);
                    // Trigger a game of Breakout
                    battleM.ChangeState(BattleState.Breakout);
                }
                break;
            case Action.Skill:
                // Open Skill Sub-Menu
                Debug.Log("No Skills Implemented yet...");
                // battleM.ChangeState(TurnState.Dialouge);
                break;
            case Action.Item:
                // Open Item Sub-Menu
                Debug.Log("No Items Implemented yet...");
                // battleM.ChangeState(TurnState.Dialouge);
                break;
        }
        
    }
}
