using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Projectile
{
    // Properties
    public float Left { get { return objInfo.MinX; }}
    public float Right { get { return objInfo.MaxX; }}
    public float Top { get { return objInfo.MaxY; }}
    public float Bottom { get { return objInfo.MinY; }}
    private void Start()
    {
        GetComponents();
        bounceCount = 5;
        lifeTime = 99f;
        Launch();
    }
    private void Update()
    {
        moveInfo.Move();
    }
    public void Reset()
    {
        moveInfo.Position = Vector2.zero;
    }

    public void Launch()
    {
        moveInfo.Direction = new Vector2(Random.value, Random.value);
    }
    public void Bounce(Vector2 direction)
    {
        moveInfo.Direction = direction;
    }
    public void BounceX()
    {
        Vector2 direction = moveInfo.Direction;
        direction.x *= -1;
        moveInfo.Direction = direction;
    }
    public void BounceY() 
    {
        Vector2 direction = moveInfo.Direction;
        direction.y *= -1;
        moveInfo.Direction = direction;
    }
}
