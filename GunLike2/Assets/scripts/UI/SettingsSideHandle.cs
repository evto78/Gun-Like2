using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSideHandle : MonoBehaviour
{
    VerticalLayoutGroup layout;
    private void Start()
    {
        layout = GetComponentInChildren<VerticalLayoutGroup>();
        layout.padding.left = -270;
    }
    void Update()
    {
        
    }
}
