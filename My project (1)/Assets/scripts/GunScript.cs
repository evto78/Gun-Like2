using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    Animator animator;
    public int maxBullets = 8;
    public int currentBullets;

    public float attackSpeed = 1;
    float attackTimer = 0;
    public float reloadSpeed = 1;
    float reloadTimer = 0;

    public float bulletSpeed = 1000;

    bool reloading = false;
    bool shooting = false;

    public GameObject pistolBullet;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = maxBullets;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reloading)
        {
            reloadTimer -= Time.deltaTime * reloadSpeed;
            if (reloadTimer <= 0)
            {
                reloading = false;
            }
        }
        if (shooting)
        {
            attackTimer -= Time.deltaTime * attackSpeed;
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

        if (Input.GetKeyDown(KeyCode.R) && !reloading && currentBullets != maxBullets)
        {
            animator.SetTrigger("Reloading");
            animator.speed = reloadSpeed;
            reloading = true;
            reloadTimer = 1;
            shooting = false;
            attackTimer = 0;
            currentBullets = maxBullets;
        }

        if (Input.GetMouseButton(0) && !reloading && !shooting)
        {
            animator.SetTrigger("Shooting");
            animator.speed = attackSpeed;
            shooting = true;
            attackTimer = 1;
            if (currentBullets > 0)
            {
                currentBullets--;

                GameObject spawnedBullet = Instantiate(pistolBullet, firePoint.position, firePoint.rotation);
                spawnedBullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed, ForceMode.Impulse);
            }
            
        }
    }
}
