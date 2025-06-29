using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float dmg;
    EnemyHealthManager ehm;
    public void SetStats(float givenDmg, EnemyHealthManager source)
    {
        dmg = givenDmg;
        ehm = source;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(dmg, false, ehm);
        }
        Destroy(gameObject);
    }
}
