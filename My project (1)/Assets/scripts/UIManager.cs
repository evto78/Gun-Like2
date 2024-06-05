using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lGunAmmo;
    public TextMeshProUGUI rGunAmmo;

    public HitScanGunScript revScript;
    public GunScript gunScript;

    // Update is called once per frame
    void Update()
    {
        lGunAmmo.text = revScript.currentBullets + " / " + revScript.maxBullets;
        rGunAmmo.text = gunScript.currentBullets + " / " + gunScript.maxBullets;
    }
}
