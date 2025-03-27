using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float dmg;
    public void SetStats(float givenDmg)
    {
        dmg = givenDmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(dmg, false);
        }
        Destroy(gameObject);
    }
}
