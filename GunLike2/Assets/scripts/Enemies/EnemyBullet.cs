using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float dmg;
    EnemyHealthManager ehm;
    string enemyName;
    public float warningDistance = 50;
    public void SetStats(float givenDmg, EnemyHealthManager source)
    {
        dmg = givenDmg;
        ehm = source;
        enemyName = ehm.data.enemyName;

        ehm.gdm.phm.uiMan.AddDangerWarnSource(transform, transform.position, false, 1);
    }

    private void Update()
    {
        if (ehm != null && Vector3.Distance(ehm.playerHM.transform.position, transform.position) < warningDistance)
        {
            ehm.gdm.phm.uiMan.AddDangerWarnSource(transform, transform.position, false, 0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            HealthManager hm = collision.gameObject.GetComponent<HealthManager>();
            if (ehm == null)
            {
                hm.TakeDamage(dmg, false, null, enemyName, transform);

            }
            else
            {
                hm.TakeDamage(dmg, false, ehm, enemyName, ehm.transform);
            }
            if (hm.curHp <= 0) { hm.gdm.unlockMan.UnlockItem(28); } //Jam dipped bullets[28] (Die to projectile)
        }
        Destroy(gameObject);
    }
}
