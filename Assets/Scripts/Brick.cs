using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectInfo))]
public class Brick : MonoBehaviour
{
    // Fields
    protected ObjectInfo objInfo;
    [SerializeField]
    protected int bounceCount;

    // Properties
    public ObjectInfo ObjectInfo { get { return objInfo; } }
    public int BounceCount { get { return bounceCount; } }

    // Start is called before the first frame update
    void Start()
    {
        objInfo = GetComponent<ObjectInfo>();
    }

    public void OnHit()
    {
        bounceCount--;
        if(bounceCount <= 0)
        {
            objInfo.IsDead = true;
        }
    }
}
