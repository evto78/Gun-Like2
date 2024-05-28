using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanGunScript : MonoBehaviour
{
    Animator animator;
    public int maxBullets = 8;
    int currentBullets;

    public float attackSpeed = 1;
    float attackTimer = 0;
    public float reloadSpeed = 1;
    float reloadTimer = 0;

    bool reloading = false;
    bool shooting = false;

    public GameObject pistolBullet;
    public Transform firePoint;

    public LayerMask mask;
    public TrailRenderer bulletTrail;

    public ParticleSystem particleSystem;
    private ParticleSystem cloneparticleSys;

    Vector3 accuracy = new Vector3(0, 0, 0);

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
            Reload();
        }

        if (Input.GetMouseButton(0) && !reloading && !shooting)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        animator.SetTrigger("Shooting");
        animator.speed = attackSpeed;
        shooting = true;
        attackTimer = 1;
        if (currentBullets > 0)
        {
            currentBullets--;

            Vector3 direction = GetDirection();
            //GameObject spawnedBullet = Instantiate(pistolBullet, firePoint.position, firePoint.rotation);
            if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, float.MaxValue, mask))
            {
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));
            }
        }
    }

    void Reload()
    {
        animator.SetTrigger("Reloading");
        animator.speed = reloadSpeed;
        reloading = true;
        reloadTimer = 1;
        shooting = false;
        attackTimer = 0;
        currentBullets = maxBullets;
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = -transform.forward;

        direction += new Vector3(
            Random.Range(-accuracy.x, accuracy.x),
            Random.Range(-accuracy.y, accuracy.y),
            Random.Range(-accuracy.z, accuracy.z)
            );

        direction.Normalize();

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;
            

            yield return null;
        }
        trail.transform.position = hit.point;
        
        cloneparticleSys = Instantiate(particleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(cloneparticleSys, 5);
        Destroy(trail.gameObject, trail.time);
    }
}
