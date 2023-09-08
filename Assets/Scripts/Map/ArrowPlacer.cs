using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPlacer : MonoBehaviour
{
    public GameObject arrowPrefab;
    GameObject PlaceArrow(Vector2 pos)
    {
        return Instantiate(arrowPrefab, pos, Quaternion.identity);
    }

    public GameObject PlaceArrowUp(Vector2 pos)
    {
        GameObject arrow = PlaceArrow(pos);
        arrow.name += " Up";
        return arrow;
    }
    public GameObject PlaceArrowDown(Vector2 pos)
    {
        GameObject arrow = PlaceArrow(pos);
        arrow.transform.Rotate(Vector3.forward, 180f);
        arrow.name += " Down";
        return arrow;
    }
    public GameObject PlaceArrowLeft(Vector2 pos)
    {
        GameObject arrow = PlaceArrow(pos);
        arrow.transform.Rotate(Vector3.forward, 90f);
        arrow.name += " Left";
        return arrow;
    }
    public GameObject PlaceArrowRight(Vector2 pos)
    {
        GameObject arrow = PlaceArrow(pos);
        arrow.transform.Rotate(Vector3.forward, -90f);
        arrow.name += " Right";
        return arrow;
    }
}
