using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Projectile
{
    private void Start()
    {
        GetComponents();
        bounceCount = 5;
        lifeTime = 99f;
        LaunchBall();
    }

    private void Update()
    {
        moveInfo.Move();
    }
    public void LaunchBall()
    {
        moveInfo.Direction = new Vector2(Random.value, Random.value);
    }
    public void BounceX()
    {
        moveInfo.BounceX();
    }
    public void BounceY() 
    {
        moveInfo.BounceY();
    }
}
