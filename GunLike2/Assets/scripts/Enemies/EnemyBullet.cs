using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float dmg;
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
            HealthManager hm = collision.gameObject.GetComponent<HealthManager>();
            hm.TakeDamage(dmg, false, ehm, ehm.data.enemyName, ehm.transform);
            if (hm.curHp <= 0) { hm.gdm.unlockMan.UnlockItem(28); } //Jam dipped bullets[28] (Die to projectile)
        }
        Destroy(gameObject);
    }
}
