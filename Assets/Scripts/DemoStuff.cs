using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoStuff : MonoBehaviour
{
    BattleManager battleM;
    // Start is called before the first frame update
    void Start()
    {
        battleM = GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(battleM.State == BattleState.Tutorial)
        {
            if(battleM.PlayerControl.ControllerButton == ControllerButton.A)
            {
                //End the tutorial
                battleM.ChangeState(BattleState.Start);
            }
        }
        else if(battleM.State == BattleState.Victory || battleM.State == BattleState.GameOver)
        {
            switch (battleM.PlayerControl.ControllerButton)
            {
                case ControllerButton.A:
                    SceneManager.LoadScene("Stage 1");
                    break;
                case ControllerButton.B:
                    Debug.Log("Exited");
                    Application.Quit();
                    break;
            }
        }
    }
}
