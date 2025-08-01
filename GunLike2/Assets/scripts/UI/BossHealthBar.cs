using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public Image fillImg;
    public EnemyHealthManager ehm;
    void Update()
    {
        fillImg.fillAmount = ehm.curHp / ehm.maxHp;
    }

}
