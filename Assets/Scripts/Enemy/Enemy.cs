using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : BattleObject
{
    // Fields
    public GameObject wall;
    // Stats
    protected int experiencePoint;
    // Mechanics
    protected bool isAttacking;
    protected string message;

    // Properties
    public int Exp { get { return experiencePoint; } set { experiencePoint = value; } }
    public bool IsAttacking { get { return isAttacking; } }
    public string Message { get { return message; } set { message = value; } }

    // Methods
    public abstract void Act();
}
