using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveInfo))]
[RequireComponent(typeof(ObjectInfo))]
public abstract class Projectile : MonoBehaviour
{
    // Fields
    protected ObjectInfo objInfo;
    protected MoveInfo moveInfo;
    [SerializeField]
    protected float lifeTime = 10f;
    [SerializeField]
    protected int bounceCount;

    // Properties
    public ObjectInfo ObjectInfo { get { return objInfo; } }
    public MoveInfo MoveInfo { get { return moveInfo; } }
    public float Lifetime { get { return lifeTime; } set { lifeTime = value; } }
    public int Bounces { get { return bounceCount; } set { bounceCount = value; } }

    protected virtual void GetComponents()
    {
        objInfo = GetComponent<ObjectInfo>();
        moveInfo = GetComponent<MoveInfo>();
    }

    protected void CheckLife()
    {
        // If life time is up, destroy this object
        if (lifeTime <= 0 || bounceCount < 0)
        {
            objInfo.IsDead = true;
        }
        lifeTime -= Time.deltaTime;
    } 
}
