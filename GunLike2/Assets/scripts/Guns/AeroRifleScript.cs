using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeroRifleScript : GunScript
{
    public override void Shoot(float bowChar)
    {
        animator.SetTrigger("Shooting");
        animator.speed = atkSpd * 1.5f;
        shooting = true;
        attackTimer = 1;
        if (currentBullets > 0)
        {
            timeSinceShot = 0f;

            int bulConsumed;
            bulConsumed = currentBullets;
            currentBullets = 0;
            dmg = 0.5f * bulConsumed * dmg;
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
