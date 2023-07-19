using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPLabel : MonoBehaviour
{
    BattleObject reference;
    TextMesh txtMesh;

    void Start()
    {
        reference = GetComponentInParent<BattleObject>();
        txtMesh = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        txtMesh.text = string.Format("{0} / {1}", reference.Health, reference.MaxHealth);
    }
}
