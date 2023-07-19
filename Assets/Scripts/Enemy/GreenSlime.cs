using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlime : Enemy
{
    // Stats
    //  HP:  4
    //  ATK: 3
    //  SPD: 2
    //  XP:  1
    // Start is called before the first frame update
    void Start()
    {
        SetStats(4,3,2);
        SetupSound();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            isDead = true; 
        }
    }
    public override void SetStats(int maxHp, int atk, int spd)
    {
        name = "Green Slime";
        experiencePoint = 1;
        base.SetStats(maxHp, atk, spd);
    }
    public override void Act()
    {
        float chance = Random.value;
        if(chance > 0.3f)
        {
            // Attack
            message = " attacks!";
            isAttacking = true;
        }
        else
        {
            message = " does nothing.";
            isAttacking = false;
        }
        //Debug.Log(name + message + "\n" + chance);
    }
}
