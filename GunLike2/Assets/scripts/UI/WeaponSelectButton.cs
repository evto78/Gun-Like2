using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectButton : MonoBehaviour
{
    TextMeshProUGUI txt; public Vector2 minMax; public int id;
    public int soundId;
    public Image txtBg; WeaponSelection weaponSelection;
    void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
        weaponSelection = GetComponentInParent<WeaponSelection>();
    }
    public void Hover(bool isHover)
    {
        if (!Application.isPlaying) { return; }
        if (isHover)
        {
            txt.color = new Color(255, 255, 255, minMax.y);
            txtBg.color = new Color(0, 0, 0, minMax.y/2f);
            weaponSelection.WeaponHover(id, soundId);
        }
        else
        {
            txt.color = new Color(255, 255, 255, minMax.x);
            txtBg.color = new Color(0, 0, 0, minMax.x/2f);
        }
    }
}
