using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public GameObject playerPanel, actionSelect, speechBox, sideBar, gameOverScreen, victoryScreen;
    public GameObject actionSelector;
    public GameObject tutorialScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public BattleManager battleM;

    // Properties
    public ActionSelector ASelector { get { return actionSelect.GetComponent<ActionSelector>(); } }
    public SpeechBox Speech { get { return speechBox.GetComponent<SpeechBox>(); } }

    private void Start()
    {
        playerPanel.GetComponent<PlayerInfoPanel>().SetUp(battleM);
    }

    // Update is called once per frame
    void Update()
    {
        if(battleM.State == BattleState.Breakout)
        {
            int bricksLeft = battleM.Breakout.BricksLeft;
            if(bricksLeft == 10)
            {
                scoreText.text = "";
            }
            else if (bricksLeft >= 7)
            {
                scoreText.text = "Okay";
            }
            else if (bricksLeft > 0)
            {
                scoreText.text = "Nice!";
            }
            else if (bricksLeft == 0)
            {
                scoreText.text = "Epic!";
            }
            
            timeText.text = string.Format("{0}", (int) battleM.Breakout.Timer);
        }
        
    }

    public void ShowUI(BattleState state)
    {
        switch (state)
        {
            case BattleState.Start:
                tutorialScreen.SetActive(false);
                gameOverScreen.SetActive(false);
                victoryScreen.SetActive(false);
                speechBox.SetActive(true);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                sideBar.SetActive(false);
                break;
            case BattleState.Tutorial:
                tutorialScreen.SetActive(true);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                break;
            case BattleState.Dialogue:
                speechBox.SetActive(true);
                actionSelect.SetActive(false);
                playerPanel.SetActive(true);
                sideBar.SetActive(false);
                break;
            case BattleState.Menu:
                speechBox.SetActive(false);
                actionSelect.SetActive(true);
                playerPanel.SetActive(true);
                sideBar.SetActive(false);
                break;
            case BattleState.Targeting:
                goto case BattleState.Dialogue;
            case BattleState.Breakout:
                speechBox.SetActive(false);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                sideBar.SetActive(true);
                break;
            case BattleState.GameOver:
                speechBox.SetActive(false);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                sideBar.SetActive(false);
                gameOverScreen.SetActive(true);
                break;
            case BattleState.Victory:
                speechBox.SetActive(false);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                sideBar.SetActive(false);
                victoryScreen.SetActive(true);
                break;
        }
    }
}
