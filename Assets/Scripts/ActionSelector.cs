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
    AudioSource selectingSound;

    [SerializeField]
    Action currentAction;
    int actionIndex;

    [SerializeField]
    RectTransform atkTransform, skillTransform, itemTransform, iTransform;
    // Properties
    public int Act { get { return actionIndex; } set { actionIndex = value; } }
    // Start is called before the first frame update
    void Start()
    {
        selectingSound = GetComponent<AudioSource>();
        atkTransform = atkText.GetComponent<RectTransform>();
        skillTransform = skillText.GetComponent<RectTransform>();
        itemTransform = itemText.GetComponent<RectTransform>();
        iTransform = indicator.GetComponent<RectTransform>();
        actionIndex = 0;
    }

    public void ChangeAction(int step)
    {
        actionIndex += step;
        if(actionIndex < 0)
        {
            actionIndex = 2;
        }
        else if(actionIndex > 2)
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

    public void ConfirmAction()
    {
        // Begin Action
        switch (currentAction)
        {
            case Action.Attack:
                // Trigger a game of Breakout
                battleM.ChangeState(TurnState.Breakout);
                break;
            case Action.Skill:
                // Open Skill Sub-Menu
                break;
            case Action.Item:
                // Open Item Sub-Menu
                break;
        }
    }
}
