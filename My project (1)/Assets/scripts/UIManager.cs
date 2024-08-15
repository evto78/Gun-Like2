using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lGunAmmoText;
    public TextMeshProUGUI rGunAmmoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI fpsText;

    public HitScanGunScript revScript;
    public GunScript gunScript;
    public HealthManager healthManager;

    int fps = 0;

    // Update is called once per frame
    void Update()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        lGunAmmoText.text = revScript.currentBullets + " / " + revScript.baseMagSize;
        rGunAmmoText.text = gunScript.currentBullets + " / " + gunScript.baseMagSize;
        healthText.text = Mathf.Round(healthManager.curHp) + " / " + Mathf.Round(healthManager.maxHp);
        fpsText.text = "FPS: " + fps;
    }
}
