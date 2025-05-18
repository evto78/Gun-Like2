using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGunScript : GunScript
{
    public override void Update()
    {
        timeSinceShot += Time.deltaTime;

        if (possession > 0 && timeSinceShot > 5f)
        {
            if (currentBullets <= 0) { AttemptReload(); }

            possessionEffect.SetActive(true);

            List<RaycastHit> hits = new List<RaycastHit>();

            hits.InsertRange(0, Physics.BoxCastAll(cam.transform.position + cam.transform.forward * 25f, Vector3.one * 10f, cam.transform.forward, cam.transform.rotation, 100f));

            EnemyHealthManager eHealthMan;
            target = null;
            foreach (RaycastHit hit in hits)
            {
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.TryGetComponent<EnemyHealthManager>(out eHealthMan))
                {
                    if ((eHealthMan.curHp + eHealthMan.armor) <= dmg)
                    {
                        target = hit.transform;
                        break;
                    }
                }

            }
            if (target != null)
            {
                AttemptShoot();
            }
        }
        else { target = null; possessionEffect.SetActive(false); }

        if (reloading)
        {
            reloadTimer -= Time.deltaTime * reSpd;
            if (reloadTimer <= 0)
            {
                reloading = false;
                currentBullets = Mathf.RoundToInt(magSize);
            }
        }
        if (shooting)
        {
            attackTimer -= Time.deltaTime * atkSpd;
            if (attackTimer <= 0)
            {
                shooting = false;
            }
        }


        if (currentBullets > 0)
        {
            animator.SetBool("NoAmmo", false);
        }
        else
        {
            animator.SetBool("NoAmmo", true);
        }

    }
}
