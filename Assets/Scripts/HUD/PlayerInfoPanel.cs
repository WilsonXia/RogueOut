using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoPanel : MonoBehaviour
{
    public GameObject hpText;
    public GameObject turnText;
    BattleManager battleM;

    TextMeshProUGUI hp;
    TextMeshProUGUI turn;

    public void SetUp(BattleManager b)
    {
        battleM = b;
    }
    // Start is called before the first frame update
    void Start()
    {
        hp = hpText.GetComponent<TextMeshProUGUI>();
        turn = turnText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = string.Format("{0}/{1}", battleM.PlayerOne.Health, battleM.PlayerOne.MaxHealth);
        turn.text = "" + battleM.TurnNumber;
    }
}
