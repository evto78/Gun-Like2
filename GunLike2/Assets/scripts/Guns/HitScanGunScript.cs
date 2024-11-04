using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanGunScript : MonoBehaviour
{
    Animator animator;
    public GunManager manager;

    //Base stats for this gun
    public float baseMagSize = 6;
    public float baseAtkSpd = 1f;
    public float baseReSpd = 1f;
    public float baseBulSpd = 9999f;
    public float baseDmg = 20f;
    public float baseAcc = 0.05f;
    public float baseBulSize = 1f;
    public int baseBulPir = 0;
    public float baseCritChance = 0f;
    public float baseCritDamage = 2f;
    public float baseWeakPointChance = 0f;
    public float baseWeakPointDamage = 2f;

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

    public float bowAct;
    public int heavySpirits;
    public int nuclearBul;
    public int introTrig;

    //Status
    float attackTimer = 0;
    float reloadTimer = 0;
    bool reloading = false;
    bool shooting = false;
    public int currentBullets;
    float bowCharge;

    bool ricochet = false;

    public GameObject pistolBullet;
    public Transform firePoint;

    public LayerMask mask;
    public TrailRenderer bulletTrail;

    public LineRenderer lr;
    float lineTimer;
    List<Vector3> linePoints = new List<Vector3>();

    public ParticleSystem particleSys;
    private ParticleSystem cloneparticleSys;

    public Camera mainCamera;
    Ray ray;
    RaycastHit hit;

    public string handThisIsIn;

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = Mathf.RoundToInt(magSize);
        animator = GetComponent<Animator>();
    }

    public void StatUpdateLeft()
    {
        handThisIsIn = "left";

        magSize = Mathf.Round(baseMagSize * manager.leftMagSize);
        atkSpd = baseAtkSpd * manager.leftAtkSpd;
        reSpd = baseReSpd * manager.leftReSpd;
        bulSpd = baseBulSpd * manager.leftBulSpd;
        dmg = baseDmg * manager.leftDmg;
        acc = baseAcc / manager.leftAcc;
        bulSize = baseBulSize * manager.leftBulSize;
        bulPir = baseBulPir + manager.leftBulPir;
        critChance = baseCritChance * manager.leftCritChance;
        critDamage = baseCritDamage * manager.leftCritDamage;
        weakPointChance = baseWeakPointChance * manager.leftWeakPointChance;
        weakPointDamage = baseWeakPointDamage * manager.leftWeakPointDamage;

        bowAct = manager.leftBowAct;
        heavySpirits = manager.leftHeavySpirit;
        nuclearBul = manager.leftNuclearBul;
        ricochet = manager.leftRicochet;
        introTrig = manager.leftIntroTrig;
    }

    public void StatUpdateRight()
    {
        handThisIsIn = "right";

        magSize = Mathf.Round(baseMagSize * manager.rightMagSize);
        atkSpd = baseAtkSpd * manager.rightAtkSpd;
        reSpd = baseReSpd * manager.rightReSpd;
        bulSpd = baseBulSpd * manager.rightBulSpd;
        dmg = baseDmg * manager.rightDmg;
        acc = baseAcc / manager.rightAcc;
        bulSize = baseBulSize * manager.rightBulSize;
        bulPir = baseBulPir + manager.rightBulPir;
        critChance = baseCritChance * manager.rightCritChance;
        critDamage = baseCritDamage * manager.rightCritDamage;
        weakPointChance = baseWeakPointChance * manager.rightWeakPointChance;
        weakPointDamage = baseWeakPointDamage * manager.rightWeakPointDamage;

        bowAct = manager.rightBowAct;
        heavySpirits = manager.rightHeavySpirit;
        nuclearBul = manager.rightNuclearBul;
        ricochet = manager.rightRicochet;
        introTrig = manager.rightIntroTrig;
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

        if(lineTimer > 0f) { lineTimer -= Time.deltaTime * atkSpd; if (lineTimer < 0f) { lineTimer = 0f; } }
        lr.startWidth = lineTimer;
        lr.endWidth = lineTimer;
    }

    public void AttemptShoot()
    {
        if ((bowAct > 0) && (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)))
        {
            bowCharge += 1 * atkSpd * Time.deltaTime;
            if (bowCharge > bowAct + 1f) { bowCharge = bowAct + 1f; }
        }
        else
        {
            if (!reloading && !shooting)
            {
                Shoot(1f);
            }
        }
    }

    public void AttemptShootUp()
    {
        if (bowAct > 0)
        {
            Shoot(bowCharge);
            bowCharge = 0f;
        }
    }

    public void AttemptReload()
    {
        if (!reloading && currentBullets != magSize)
        {
            Reload();
        }
    }

    void Shoot(float bowChar)
    {
        animator.SetTrigger("Shooting");
        animator.speed = atkSpd + atkSpd / 10f;
        shooting = true;
        attackTimer = 1;
        if (currentBullets > 0)
        {
            currentBullets--;

            Vector3 direction = GetDirection(bowChar);
            
            lr.positionCount = 1;
            lr.SetPosition(0, firePoint.position);
            linePoints.Clear();
            linePoints.Add(firePoint.position);

            if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, float.MaxValue, mask))
            {
                ApplyDamageAndImpact(hit, bowChar);

                linePoints.Add(hit.point);

                if (bulPir > 0) { PierceAndRico(bowChar, bulPir, ray, hit, direction); }
                else
                {
                    RenderLine();
                }

            }
            else
            {
                linePoints.Add(firePoint.position + direction * 9999);
                RenderLine();
            }
        }
    }

    void PierceAndRico(float bowChar, int pierceLeft, Ray givenRay, RaycastHit givenHit, Vector3 givenRayDir)
    {
        Debug.Log(pierceLeft);
        pierceLeft--;

        if (ricochet)
        {
            Vector3 reflectDir = Vector3.Reflect(givenRayDir, givenHit.normal);

            Ray newRay = new Ray(givenHit.point, reflectDir);
            RaycastHit newHit;

            Debug.DrawRay(givenHit.point, reflectDir, Color.magenta, 10f);
            Debug.DrawLine(givenHit.point, givenHit.point + Vector3.up * 5f, Color.green, 10f);
            Debug.DrawLine(givenHit.point + reflectDir, givenHit.point + reflectDir + Vector3.up * 5f, Color.white, 10f);

            if (Physics.Raycast(newRay, out newHit, float.MaxValue, mask))
            {
                ApplyDamageAndImpact(newHit, bowChar); 

                linePoints.Add(newHit.point);
                Debug.Log(newHit.point);

                if (pierceLeft > 0) { PierceAndRico(bowChar, pierceLeft, newRay, newHit, reflectDir); }
                else
                {
                    RenderLine();
                }
            }
            else
            {
                linePoints.Add(givenHit.point + reflectDir * 9999);
                RenderLine();
            }
        }
        else
        {
            Vector3 reflectDir = givenRayDir;

            Ray newRay = new Ray(givenHit.point, reflectDir);
            RaycastHit newHit;

            Debug.DrawRay(givenHit.point, reflectDir, Color.magenta, 10f);
            Debug.DrawLine(givenHit.point, givenHit.point + Vector3.up * 5f, Color.green, 10f);
            Debug.DrawLine(givenHit.point+reflectDir, givenHit.point+reflectDir + Vector3.up * 5f, Color.white, 10f);
            if (Physics.Raycast(newRay, out newHit, float.MaxValue, mask))
            {

                ApplyDamageAndImpact(newHit, bowChar);

                linePoints.Add(newHit.point);
                Debug.Log(newHit.point);

                if (pierceLeft > 0) { PierceAndRico(bowChar, pierceLeft, newRay, newHit, reflectDir); }
                else
                {
                    RenderLine();
                }
            }
            else
            {
                linePoints.Add(givenHit.point + reflectDir * 9999);
                RenderLine();
            }
        }

        
    }

    //Does weak point detection, crit chance, damage application, and all items that deal with on hit.
    void ApplyDamageAndImpact(RaycastHit hit, float bowChar)
    {
        if (hit.collider.gameObject.CompareTag("Enemy"))
        {
            if (Random.Range(1, 100) < critChance)
            {
                if (Random.Range(1, 100) < weakPointChance)
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(dmg * critDamage * weakPointDamage * bowChar, false, "critWeakHit", hit.point, handThisIsIn);

                    hit.collider.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(dmg * critDamage * bowChar, false, "critHit", hit.point, handThisIsIn);

                    hit.collider.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                if (Random.Range(1, 100) < weakPointChance)
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(dmg * weakPointDamage * bowChar, false, "weakHit", hit.point, handThisIsIn);

                    hit.collider.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(dmg * bowChar, false, "normalHit", hit.point, handThisIsIn);

                    hit.collider.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
            }

            if ((hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().curHp / hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().maxHp) * 100f <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().Die();
            }

            if (nuclearBul > 0)
            {
                if (Random.Range(1, 100) <= (25 + 5 * nuclearBul))
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().TakePercentDamage(0.15f);
                }
            }
        }
        if (hit.collider.gameObject.CompareTag("EnemyWeakPoint"))
        {
            if (Random.Range(1, 100) < critChance)
            {
                hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(dmg * critDamage * weakPointDamage, false, "critWeakHit", hit.point, handThisIsIn);

                hit.collider.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(dmg * weakPointDamage, false, "weakHit", hit.point, handThisIsIn);

                hit.collider.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }

            if ((hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().curHp / hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().maxHp) * 100f <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().Die();
            }

            if (nuclearBul > 0)
            {
                if (Random.Range(1, 100) <= (25 + 5 * nuclearBul))
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>().TakePercentDamage(0.15f);
                }
            }
        }
    }

    void Reload()
    {
        animator.SetTrigger("Reloading");
        animator.speed = reSpd;
        reloading = true;
        reloadTimer = 1;
        shooting = false;
        attackTimer = 0;
        //currentBullets = Mathf.RoundToInt(magSize);
    }

    private Vector3 GetDirection(float bowChar)
    {
        Vector3 direction = -transform.forward;

        acc = acc / bowChar;

        direction += new Vector3(
            Random.Range(-acc, acc),
            Random.Range(-acc, acc),
            Random.Range(-acc, acc)
            );

        direction.Normalize();

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;


            yield return null;
        }
        trail.transform.position = hit.point;

        cloneparticleSys = Instantiate(particleSys, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(cloneparticleSys.gameObject, 5);
        Destroy(trail.gameObject, trail.time);
    }

    void RenderLine()
    {
        lineTimer = 0.3f;

        lr.positionCount = linePoints.Count;

        for (int i = 0; i < linePoints.Count; i++)
        {
            Debug.DrawLine(linePoints[i], linePoints[i] + Vector3.up * 3f, Color.yellow, 10f);
            lr.SetPosition(i, linePoints[i]);
        }
    }
}