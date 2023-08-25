using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : BattleObject
{
    public static PlayerData instance;
    // public List<Skill> skills;
    private void Start()
    {
        name = "Player";
        SetStats(15, 3, 5);
        SetupSound();
    }
    private void Awake()
    {
       /* Name: Player
        * HP: 15
        * Atk: 3
        * Speed: 5
        */
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
