using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    Animator animator;
    public GunManager manager;

    //Base stats for this gun
    public float baseMagSize = 8;
    public float baseAtkSpd = 1f;
    public float baseReSpd = 1f;
    public float baseBulSpd = 1000f;
    public float baseDmg = 10f;
    public float baseAcc = 15f;
    public float baseBulSize = 1f;
    public int baseBulPir = 0;
    public float baseCritChance = 0f;
    public float baseCritDamage = 2f;
    public float baseWeakPointChance = 0f;
    public float baseWeakPointDamage = 1.5f;

    //Modified Stats
    public float magSize;
    public float atkSpd;
    public float reSpd;
    public float bulSpd;
    public float dmg;
    public float acc;
    public float bulSize;
    public int bulPir;
    public float critChance;
    public float critDamage;
    public float weakPointChance;
    public float weakPointDamage;

    //Status
    float reloadTimer = 0;
    float attackTimer = 0;
    public int currentBullets;
    bool reloading = false;
    bool shooting = false;

    public GameObject pistolBullet;
    public Transform firePoint;

    public Camera cam;

    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = Mathf.RoundToInt(magSize);
        animator = GetComponent<Animator>();
    }

    public void StatUpdateLeft()
    {
        magSize = Mathf.Round(baseMagSize * manager.leftMagSize);
        atkSpd = baseAtkSpd * manager.leftAtkSpd;
        reSpd = baseReSpd * manager.leftReSpd;
        bulSpd = baseBulSpd * manager.leftBulSpd;
        dmg = baseDmg * manager.leftDmg;
        acc = baseAcc * manager.leftAcc;
        bulSize = baseBulSize * manager.leftBulSize;
        bulPir = baseBulPir + manager.leftBulPir;
        critChance = baseCritChance * manager.leftCritChance;
        critDamage = baseCritDamage * manager.leftCritDamage;
        weakPointChance = baseWeakPointChance * manager.leftWeakPointChance;
        weakPointDamage = baseWeakPointDamage * manager.leftWeakPointDamage;
    }

    public void StatUpdateRight()
    {
        magSize = Mathf.Round(baseMagSize * manager.rightMagSize);
        atkSpd = baseAtkSpd * manager.rightAtkSpd;
        reSpd = baseReSpd * manager.rightReSpd;
        bulSpd = baseBulSpd * manager.rightBulSpd;
        dmg = baseDmg * manager.rightDmg;
        acc = baseAcc * manager.rightAcc;
        bulSize = baseBulSize * manager.rightBulSize;
        bulPir = baseBulPir + manager.rightBulPir;
        critChance = baseCritChance * manager.rightCritChance;
        critDamage = baseCritDamage * manager.rightCritDamage;
        weakPointChance = baseWeakPointChance * manager.rightWeakPointChance;
        weakPointDamage = baseWeakPointDamage * manager.rightWeakPointDamage;
    }

    // Update is called once per frame
    void Update()
    {
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

    public void AttemptShoot()
    {
        if (!reloading && !shooting)
        {
            Shoot();
        }
    }

    public void AttemptReload()
    {
        if (!reloading && currentBullets != magSize)
        {
            Reload();
        }
    }

    public void Reload()
    {
        animator.SetTrigger("Reloading");
        animator.speed = reSpd;
        reloading = true;
        reloadTimer = 1;
        shooting = false;
        attackTimer = 0;
    }

    public void Shoot()
    {
        animator.SetTrigger("Shooting");
        animator.speed = atkSpd;
        shooting = true;
        attackTimer = 1;
        if (currentBullets > 0)
        {
            currentBullets--;

            GameObject spawnedBullet = Instantiate(pistolBullet, firePoint.position, firePoint.rotation);
            spawnedBullet.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
            //spawnedBullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulSpd, ForceMode.Impulse);

            spawnedBullet.GetComponent<BulletScript>().mainCamera = cam;
            if (Random.Range(1, 100) < critChance)
            {
                if (Random.Range(1, 100) < weakPointChance)
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(dmg * critDamage * weakPointDamage, true, bulPir, true, weakPointDamage, bulSpd);
                }
                else
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(dmg * critDamage, true, bulPir, false, weakPointDamage, bulSpd);
                }
            }
            else
            {
                if (Random.Range(1, 100) < weakPointChance)
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(dmg * weakPointDamage, false, bulPir, true, weakPointDamage, bulSpd);
                }
                else
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(dmg, false, bulPir, false, weakPointDamage, bulSpd);
                }
            }

        }
    }
}
