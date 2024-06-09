using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public int maxHp = 100;
    public float curHp;
    public float armor = 5;
    public float healthRegen = 1f;
    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
    }
    public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems)
    {
        healthRegen = 1f * ((givenLeftItems[2]/10f + 1f) + (givenRightItems[2]/10f + 1f));
        armor = 5f * (((givenLeftItems[3]-1f)/25f + 1f) + ((givenRightItems[3]-1f)/25f + 1f));
        
    }
    void Update()
    {
        if (curHp < maxHp)
        {
            curHp += healthRegen * Time.deltaTime;    

        }
    }

    public void TakeDamage(float damageTaken)
    {
        if (damageTaken <= 0)
        {
            //Heal
            curHp += damageTaken;
        }
        else
        {
            //Damage
            if (damageTaken <= armor)
            {
                //armor has absorbed all damage but min dmg is 1
                curHp -= 1f;
            }
            else
            {
                //return new hp with dmg reduced by armor
                curHp -= (damageTaken - armor);
            }
        }
    }
}