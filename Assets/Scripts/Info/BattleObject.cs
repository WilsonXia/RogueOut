using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleObject : MonoBehaviour
{
    // Fields
    new protected string name;
    protected int maxHealth;
    protected int health;
    protected int attack;
    protected int speed;
    protected bool isDead;

    // Sounds
    AudioSource attackSound;

    // Properties
    public string Name { get { return name; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int Health { get { return health; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Speed { get { return speed; } set { speed = value; } }
    public bool IsDead { get { return isDead; } }

    // Methods
    public virtual void SetStats(int maxHp, int atk, int spd)
    {
        maxHealth = maxHp;
        health = maxHealth;
        attack = atk;
        speed = spd;
        isDead = false;
    }
    public void SetupSound()
    {
        attackSound = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        attackSound.Play();
    }
    public void TakeDamage(int damage) 
    {
        if(damage > health)
        {
            health = 0;
            isDead = true;
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
