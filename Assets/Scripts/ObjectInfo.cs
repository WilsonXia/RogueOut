using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    SpriteRenderer sprite;
    [SerializeField]
    float minX;
    [SerializeField]
    float maxX;
    [SerializeField]
    float minY;
    [SerializeField]
    float maxY;
    [SerializeField]
    float radius;
    [SerializeField]
    float width;
    [SerializeField]
    float height;
    bool isHostile = false;
    [SerializeField]
    bool isDead = false;
    public float MaxX { get { return maxX; } }
    public float MaxY { get { return maxY; } }
    public float MinX { get { return minX; } }
    public float MinY { get { return minY; } }
    public float Radius { get { return radius; } }
    public float Width { get { return width; } }
    public float Height { get { return height; } }
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
        // Set up measurements
        radius = ((maxX - minX) + (maxY - minY)) / 4;
        width = (maxX - minX);
        height = (maxY - minY);

        isDead = false;
        isHostile = false;
    }

    void Update()
    {
        maxX = sprite.bounds.max.x;
        maxY = sprite.bounds.max.y;
        minX = sprite.bounds.min.x;
        minY = sprite.bounds.min.y;
    }
}
