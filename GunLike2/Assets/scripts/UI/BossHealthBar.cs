using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public Image fillImg;
    public EnemyHealthManager ehm;
    float scaleprog;
    void Update()
    {
        scaleprog += Time.deltaTime*2f;
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.75f, scaleprog);
        fillImg.fillAmount = ehm.curHp / ehm.maxHp;
    }
    private void OnEnable()
    {
        scaleprog = 0;
    }
}
