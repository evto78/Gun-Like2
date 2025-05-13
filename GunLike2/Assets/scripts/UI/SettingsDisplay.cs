using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsDisplay : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public Slider target;
    void Update()
    {
        txt.text = Mathf.Round(target.value * 100f).ToString();
    }
}
