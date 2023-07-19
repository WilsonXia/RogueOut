using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchText : MonoBehaviour
{
    Ball ball;
    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponentInParent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.Launched)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
