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
    public TextMeshProUGUI effectsText; //   <---- Changed by heath manager script since it holds the effect info.
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI velocityText;

    public HitScanGunScript revScript;
    public GunScript gunScript;
    public HealthManager healthManager;

    public GameObject damageText;
    public GameObject myCanvas;

    int fps = 0;

    // Update is called once per frame
    void Update()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        lGunAmmoText.text = revScript.currentBullets + " / " + revScript.magSize;
        rGunAmmoText.text = gunScript.currentBullets + " / " + gunScript.magSize;
        healthText.text = Mathf.Round(healthManager.curHp) + " / " + Mathf.Round(healthManager.maxHp);
        fpsText.text = "FPS: " + fps;
    }

    public void PopUpText(float dmgTaken, string givenColor, Vector3 worldPos)
    {
        GameObject spawnedText = Instantiate(damageText);

        spawnedText.transform.SetParent(myCanvas.transform);

        spawnedText.GetComponent<DamageText>().SetText(dmgTaken + "", givenColor, worldPos) ;

        TextMeshProUGUI theText = spawnedText.GetComponent<TextMeshProUGUI>();

        theText.text = ""+dmgTaken;
    }
}
