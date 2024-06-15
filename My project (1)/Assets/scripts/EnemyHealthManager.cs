using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class EnemyHealthManager : MonoBehaviour
{
    
    public float curHp;
    public float maxHp;

    public float armor;

    public GameObject damageText;

    void Start()
    {
        curHp = maxHp;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float dmgTaken, bool ignoreArmor, string textColor, Transform hitLocation)
    {
        if (ignoreArmor)
        {
            curHp -= dmgTaken;
            PopUpText(Mathf.RoundToInt(curHp -= dmgTaken), textColor, hitLocation);
        }
        else
        {
            if (armor >= dmgTaken)
            {
                curHp -= 1f;
                PopUpText(1, textColor, hitLocation);
            }
            else
            {
                curHp -= (dmgTaken - armor);
                PopUpText(Mathf.RoundToInt(dmgTaken - armor), textColor, hitLocation);
            }
        }
    }

    void PopUpText(int dmgText, string textColor, Transform hitLocation)
    {
        GameObject spawnedText = Instantiate(damageText);
        spawnedText.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        spawnedText.GetComponent<DamageText>().SetText(dmgText.ToString(), textColor);
    }
}
