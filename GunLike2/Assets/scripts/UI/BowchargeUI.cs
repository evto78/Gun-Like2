using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowchargeUI : MonoBehaviour
{
    GunManager gm;
    public Image leftCharge;
    public Image rightCharge;
    public Gradient chargeColors;
    public Color opacity;
    void Start()
    {
        gm = GetComponentInParent<GunManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.leftBowAct + gm.rightBowAct <= 0 || gm.leftGunScript == null || gm.rightGunScript == null) { return; }
        leftCharge.fillAmount = gm.leftGunScript.bowCharge / (gm.leftGunScript.bowAct + 1f); leftCharge.color = chargeColors.Evaluate(leftCharge.fillAmount);
        leftCharge.color = new Color(leftCharge.color.r, leftCharge.color.g, leftCharge.color.b, opacity.a);
        rightCharge.fillAmount = gm.rightGunScript.bowCharge / (gm.rightGunScript.bowAct + 1f); rightCharge.color = chargeColors.Evaluate(rightCharge.fillAmount);
        rightCharge.color = new Color(rightCharge.color.r, rightCharge.color.g, rightCharge.color.b, opacity.a);

    }
}
