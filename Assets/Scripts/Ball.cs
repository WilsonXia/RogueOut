using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Projectile
{
    AudioSource ballSound;
    bool hasLaunched;
    // Properties
    public float Left { get { return objInfo.MinX; }}
    public float Right { get { return objInfo.MaxX; }}
    public float Top { get { return objInfo.MaxY; }}
    public float Bottom { get { return objInfo.MinY; }}
    public bool Launched { get { return hasLaunched; }}
    private void Start()
    {
        GetComponents();
        hasLaunched = false;
        ballSound = GetComponent<AudioSource>();
        bounceCount = 5;
        lifeTime = 99f;
    }
    private void Update()
    {
        moveInfo.Move();
    }
    public void Reset()
    {
        moveInfo.Reset();
        moveInfo.Position = new Vector2(0,-3);
        transform.position = new Vector2(0,-3);
        hasLaunched = false;
    }

    public void Launch()
    {
        //moveInfo.Direction = new Vector2(Random.value, Random.value);
        moveInfo.Direction = Vector2.up;
        hasLaunched = true;
    }
    public void Bounce(Vector2 direction)
    {
        moveInfo.Direction = direction;
        ballSound.Play();
    }
    public void BounceX()
    {
        Vector2 direction = moveInfo.Direction;
        direction.x *= -1;
        moveInfo.Direction = direction;
        ballSound.Play();
    }
    public void BounceY() 
    {
        Vector2 direction = moveInfo.Direction;
        direction.y *= -1;
        moveInfo.Direction = direction;
        ballSound.Play();
    }
}
