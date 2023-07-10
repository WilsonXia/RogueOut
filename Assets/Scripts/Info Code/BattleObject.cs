using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleObject : MonoBehaviour
{
    // Fields
    protected int maxHealth;
    protected int health;
    protected int attack;
    protected int speed;
    protected bool isDead;

    // Properties
    public int MaxHealth { get { return maxHealth; } }
    public int Health { get { return health; } }
    public int Attack { get { return attack; } }
    public int Speed { get { return speed; } }
    public bool IsDead { get { return isDead; } }

    // Methods
    public void TakeDamage(int damage) 
    {
        if(damage > health)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }
    }

    public void Heal(int heal) 
    {
        health += heal;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
