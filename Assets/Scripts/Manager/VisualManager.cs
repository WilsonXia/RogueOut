using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void SingleSelectedAnimation(GameObject animatingObject)
    {
        Color selected = animatingObject.GetComponent<SpriteRenderer>().color;
        float changingValue = Mathf.PingPong(Time.time, 0.4f) + 0.6f;
        animatingObject.GetComponent<SpriteRenderer>().color = new Color(selected.r, selected.g, selected.b, changingValue);
    }
}
