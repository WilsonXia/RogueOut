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
        scoreText.text = string.Format("{0}", battleM.Breakout.BricksLeft);
    }

    public void ShowUI(BattleState state)
    {
        switch (state)
        {
            case BattleState.Tutorial:
                tutorialScreen.SetActive(true);
                break;
            case BattleState.Dialogue:
                tutorialScreen.SetActive(false);
                speechBox.SetActive(true);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                sideBar.SetActive(false);
                gameOverScreen.SetActive(false);
                victoryScreen.SetActive(false);
                break;
            case BattleState.Menu:
                speechBox.SetActive(false);
                actionSelect.SetActive(true);
                playerPanel.SetActive(true);
                sideBar.SetActive(false);
                gameOverScreen.SetActive(false);
                victoryScreen.SetActive(false);
                break;
            case BattleState.Breakout:
                speechBox.SetActive(false);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                sideBar.SetActive(true);
                gameOverScreen.SetActive(false);
                victoryScreen.SetActive(false);
                break;
            case BattleState.GameOver:
                speechBox.SetActive(false);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                sideBar.SetActive(false);
                gameOverScreen.SetActive(true);
                victoryScreen.SetActive(false);
                break;
            case BattleState.Victory:
                speechBox.SetActive(false);
                actionSelect.SetActive(false);
                playerPanel.SetActive(false);
                sideBar.SetActive(false);
                gameOverScreen.SetActive(false);
                victoryScreen.SetActive(true);
                break;
        }
    }
}