using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectorController : MonoBehaviour
{
    PlayerController player;
    ActionSelector action;
    public void SetUp(PlayerController p, ActionSelector a) 
    {
        player = p;
        action = a;
    }

    // Update is called once per frame
    void Update()
    {
        if(action.BattleManager.State == BattleState.Targeting) 
        {
            TargetSelect();
        }
        else if(action.BattleManager.State == BattleState.Menu)
        {
            MenuSelect();
        }
    }

    void MenuSelect()
    {
        switch (player.ControllerDirection)
        {
            case ControllerDirection.Down:
                action.ChangeAction(1);
                break;
            case ControllerDirection.Up:
                action.ChangeAction(-1);
                break;
            default:
                break;
        }
        switch (player.ControllerButton)
        {
            case ControllerButton.A:
                action.SelectAction();
                break;
            case ControllerButton.B:
                break;
            default:
                break;
        }
    }
    void TargetSelect()
    {
        switch (player.ControllerDirection)
        {
            case ControllerDirection.Right:
                if(player.Pressed())
                action.ChangeTarget(1);
                break;
            case ControllerDirection.Left:
                if(player.Pressed())
                action.ChangeTarget(-1);
                break;
            default:
                break;
        }
        switch (player.ControllerButton)
        {
            case ControllerButton.A:
                action.SelectTarget();
                break;
            case ControllerButton.B:
                break;
            default:
                break;
        }
    }
}
