using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    // Stats
    //  HP:  6
    //  ATK: 2
    //  SPD: 6
    //  XP:  1
    // Start is called before the first frame update
    void Start()
    {
        SetStats(6, 2, 6);
        SetupSound();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            isDead = true;
        }
    }
    public override void SetStats(int maxHp, int atk, int spd)
    {
        name = "Bat";
        experiencePoint = 1;
        base.SetStats(maxHp, atk, spd);
    }
    public override void Act()
    {
        float chance = Random.value;
        if (chance > 0.3f)
        {
            // Attack
            message = " attacks!";
            isAttacking = true;
            PlaySound();
        }
        else
        {
            message = " flys around and squeaks.";
            isAttacking = false;
        }
    }
}
