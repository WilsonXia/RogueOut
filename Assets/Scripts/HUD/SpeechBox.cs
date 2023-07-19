using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBox : MonoBehaviour
{
    TextMeshProUGUI textBox;
    string message;

    private void Start()
    {
        textBox = GetComponentInChildren<TextMeshProUGUI>();
        message = "Battle Start!";
    }
    void Update()
    {
        textBox.text = message;
    }

    public void SetMessage(string m) 
    {
        message = m;
    }
}
