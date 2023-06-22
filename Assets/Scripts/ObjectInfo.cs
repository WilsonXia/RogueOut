using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    SpriteRenderer sprite;
    float maxX;
    float maxY;
    float minX;
    float minY;
    float radius;
    bool isHostile = false;
    bool isDead;
    public float MaxX { get { return maxX; } }
    public float MaxY { get { return maxY; } }
    public float MinX { get { return minX; } }
    public float MinY { get { return minY; } }
    public float Radius { get { return radius; } }
    public bool IsHostile { get { return isHostile; } set { isHostile = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    void Start()
    {
        // Get the current GameObject's SpriteRenderer
        sprite = GetComponent<SpriteRenderer>();
        // Set up the min and max bounds
        maxX = sprite.bounds.max.x;
        maxY = sprite.bounds.max.y;
        minX = sprite.bounds.min.x;
        minY = sprite.bounds.min.y;
        // Set up radius
        radius = ((maxX - minX) + (maxY - minY)) / 4;
    }

    void Update()
    {
        maxX = sprite.bounds.max.x;
        maxY = sprite.bounds.max.y;
        minX = sprite.bounds.min.x;
        minY = sprite.bounds.min.y;
    }
}
