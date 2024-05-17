using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Android;

public class HealthManager : MonoBehaviour
{
    public int maxHp = 100;
    public float curHp;
    
    public float HealthRegen = 1;
    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
    
    }
    public void StatUpdate(List<int> givenItems)
    {
        HealthRegen = 1 * (givenItems[2]/10 + 1);
        
    }
    void Update()
    {
        if (curHp < maxHp)
        {
            curHp += HealthRegen * Time.deltaTime;    
            Debug.Log(curHp);
        }
        
        
    }
    private void OnTriggerEnter(Collider collision) 
    {
        if(collision.gameObject.tag == "HurtBox")
        {
            curHp -= 2;
        }
    }
}