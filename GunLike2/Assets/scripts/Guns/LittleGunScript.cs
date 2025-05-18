using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGunScript : GunScript
{
    public float littleCharge;

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
            littleCharge = 0;
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

    public override void Shoot(float bowChar)
    {
        animator.SetTrigger("Shooting");
        animator.speed = atkSpd * 1.5f;
        shooting = true;
        attackTimer = 1;
        if (currentBullets > 0)
        {


            timeSinceShot = 0f;

            littleCharge += 0.1f;
            currentBullets--;

            GameObject spawnedBullet = Instantiate(pistolBullet, firePoint.position, firePoint.rotation);
            if (target != null) { spawnedBullet.transform.LookAt(target); timeSinceShot = 5f; }
            acc = acc / bowChar;
            spawnedBullet.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
            //spawnedBullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulSpd, ForceMode.Impulse);

            spawnedBullet.GetComponent<BulletScript>().mainCamera = cam;

            GameObject spawnedBulletB = Instantiate(pistolBullet, firePoint.position, firePoint.rotation);
            if (introTrig > 0)
            {
                spawnedBullet.GetComponent<BulletScript>().IntroTrigSetUp(spawnedBulletB, true);
                spawnedBulletB.GetComponent<BulletScript>().IntroTrigSetUp(spawnedBullet, false);
            }
            else
            {
                Destroy(spawnedBulletB);
            }

            if (Random.Range(1, 100) < critChance)
            {
                if (Random.Range(1, 100) < weakPointChance)
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(dmg * critDamage * weakPointDamage * bowChar, true, bulPir, true, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon, fastSpon * 10f, largeSpon * 5f, advTrig);
                }
                else
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(dmg * critDamage * bowChar, true, bulPir, false, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon, fastSpon * 10f, largeSpon * 5f, advTrig);
                }
            }
            else
            {
                if (Random.Range(1, 100) < weakPointChance)
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(dmg * weakPointDamage * bowChar, false, bulPir, true, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon, fastSpon * 10f, largeSpon * 5f, advTrig);
                }
                else
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(dmg * bowChar, false, bulPir, false, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon, fastSpon * 10f, largeSpon * 5f, advTrig);
                }
            }

            if (introTrig > 0)
            {
                acc = acc / bowChar;
                if (masterTrig > 0)
                {
                    acc += 2f;
                }
                else
                {
                    acc += 4;
                }
                spawnedBulletB.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
                //spawnedBullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulSpd, ForceMode.Impulse);

                spawnedBulletB.GetComponent<BulletScript>().mainCamera = cam;
                if (Random.Range(1, 100) < critChance)
                {
                    if (Random.Range(1, 100) < weakPointChance)
                    {
                        spawnedBulletB.GetComponent<BulletScript>().setStats(dmg * critDamage * weakPointDamage * bowChar, true, bulPir, true, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon, fastSpon * 10f, largeSpon * 5f, advTrig);
                    }
                    else
                    {
                        spawnedBulletB.GetComponent<BulletScript>().setStats(dmg * critDamage * bowChar, true, bulPir, false, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon, fastSpon * 10f, largeSpon * 5f, advTrig);
                    }
                }
                else
                {
                    if (Random.Range(1, 100) < weakPointChance)
                    {
                        spawnedBulletB.GetComponent<BulletScript>().setStats(dmg * weakPointDamage * bowChar, false, bulPir, true, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon, fastSpon * 10f, largeSpon * 5f, advTrig);
                    }
                    else
                    {
                        spawnedBulletB.GetComponent<BulletScript>().setStats(dmg * bowChar, false, bulPir, false, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon, fastSpon * 10f, largeSpon * 5f, advTrig);
                    }
                }
            }

        }
    }
}
