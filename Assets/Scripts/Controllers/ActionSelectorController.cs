using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectorController : MonoBehaviour
{
    Player player;
    ActionSelector action;
    public void SetUp(Player p, ActionSelector a) 
    {
        player = p;
        action = a;
    }

    // Update is called once per frame
    void Update()
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
        switch(player.ControllerButton)
        {
            case ControllerButton.A:
                action.ConfirmAction();
                break;
            case ControllerButton.B:
                break;
            default:
                break;
        }
    }
}
