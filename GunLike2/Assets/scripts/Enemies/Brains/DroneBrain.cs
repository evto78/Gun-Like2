using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBrain : MonoBehaviour
{
    public float hoverHeight; Ray hoverRay;
    public float hoverSpeed;
    public float speed;
    float curHeight;
    Rigidbody rb;
    public List<PropellerSpin> propellers;
    EnemyHealthManager hm; HealthManager phm;
    EnemyHealthManager grabableTarget;
    GameObject player;
    enum holdType { empty, uzi, grenade, nuke, crab} holdType holding;
    public enum state { wander, seeking, attacking} public state curState;
    bool jammed;
    GameObject fop;
    [Header("Uzi Walker")]
    public GameObject pickUpUzi; public GameObject uziBullet; public Transform firePointUzi; public GameObject gunUzi; public ParticleSystem jammedUzi;
    public float uziCooldown; float uCooldownTimer; public float uziBurstCooldown; float uBurstTimer; public int UziBustAmt; int bulShot; public float uziAcc; public float uziBulSpeed;
    [Header("Grenade Lobber")]
    public GameObject pickUpGrenade; public GameObject grenade; public Transform firePointGrenade; public GameObject gunGrenade; public ParticleSystem jammedGrenade;
    public float greCooldown; float gCooldownTimer; public float greBurstCooldown; float gBurstTimer; public int greBurstAmt; int greShot; public int maxAtOnce; int curAmt; public float greLaunchSpeed;
    List<EnemyHealthManager> activeGernades = new List<EnemyHealthManager>();
    [Header("Nukeshell Spider")]
    public GameObject pickUpNuke; public GameObject nuke; Vector3 nukeDivePos; public float nukeSpeed; public float nukeHoverSpeed;
    [Header("Crate Crab")]
    public GameObject pickUpCrab; public GameObject crabBullet; public Transform firePointCrab; public ParticleSystem jammedCrab; public float crabBulSpeed;
    public float crabCooldown; float cCooldownTimer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hm = GetComponent<EnemyHealthManager>();
        holding = holdType.empty;
        curState = state.wander;
        phm = hm.gdm.phm;
        grabableTarget = null;
        player = phm.gameObject;
        fop = GameObject.Find("FlyingOrbitPoint");
    }
    void Update()
    {
        jammed = hm.activeEffects[30].x > 0;
        if (phm.activeEffects[22].x > 0 && !hm.gdm.pointsLocked)
        {
            curState = state.wander;
        }
        else if(holding == holdType.empty)
        {
            curState = state.seeking;
        }
        else
        {
            curState = state.attacking;
        }
        for(int i = 0; i < activeGernades.Count; i++)
        {
            EnemyHealthManager ehm = activeGernades[i];
            if(ehm == null) { activeGernades.RemoveAt(i); }
        } curAmt = activeGernades.Count;
        switch (curState)
        {
            case state.wander: break;
            case state.seeking:
                //if no target is set, set a target
                if(holding == holdType.empty && grabableTarget == null) 
                { 
                    grabableTarget = FindActiveWalker();
                    if (grabableTarget == null)
                    {
                        MoveToTarget(fop.transform.position,1);
                        if (fop.transform.position.y > transform.position.y) { hoverHeight += 5 * Time.deltaTime; }
                        else { hoverHeight -= 5 * Time.deltaTime; }
                    }
                    else { hoverHeight = 50; }
                }
                //if a target is set, but not holding it yet, move towards the target
                else if(holding == holdType.empty && grabableTarget != null){
                    MoveToTarget(grabableTarget.transform.position, 0);
                    //if above the target, lower and get closer.
                    if (Vector2.Distance(new Vector2(transform.position.x,transform.position.z),new Vector2(grabableTarget.transform.position.x,grabableTarget.transform.position.z)) < 5f) 
                    { hoverHeight = 2; }
                    //if on the target, destroy them, and upgrade this drone.
                    if(Vector3.Distance(transform.position, grabableTarget.transform.position) < 10f)
                    {
                        switch (grabableTarget.data.enemyName)
                        {
                            case "Uzi Walker": holding = holdType.uzi; pickUpUzi.SetActive(true); break;
                            case "Balistic Basalisk": holding = holdType.uzi; pickUpUzi.SetActive(true); break;
                            case "Crate Crab": holding = holdType.crab; pickUpCrab.SetActive(true); break;
                            case "Grenade Lobber": holding = holdType.grenade; pickUpGrenade.SetActive(true); break;
                            case "Nukeshell Spider": holding = holdType.nuke; pickUpNuke.SetActive(true); nukeDivePos = player.transform.position; break;
                        }
                        hoverHeight = 50f;
                        Destroy(grabableTarget.gameObject);
                        curState = state.attacking;
                    }
                }
                break;
            case state.attacking:
                switch (holding)
                {
                    case holdType.empty: break;
                    case holdType.uzi:
                        MoveToTarget(player.transform.position, 30);
                        AttemptShoot();
                        break;
                    case holdType.grenade:
                        MoveToTarget(player.transform.position, 40);
                        AttemptShoot();
                        break;
                    case holdType.nuke:
                        MoveToTarget(nukeDivePos, 0);
                        hoverSpeed = nukeHoverSpeed;
                        if(nukeDivePos.y > transform.position.y) { hoverHeight += 5 * Time.deltaTime; }
                        else { hoverHeight -= 5 * Time.deltaTime; }
                        break;
                    case holdType.crab:
                        MoveToTarget(player.transform.position, 40);
                        AttemptShoot();
                        break;
                }
                break;
        }
        DistanceToGround();
        float webbedSpeedMod = 1f;
        if (hm.activeEffects[39].x > 0) { webbedSpeedMod = 1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136])); }

        if (curHeight < hoverHeight)
        {
            rb.AddForce(Vector3.up * hoverSpeed * webbedSpeedMod * Time.deltaTime);
            UpdatePropellerSpeed();
        }
        else
        {
            rb.AddForce(Vector3.up * hoverSpeed * webbedSpeedMod * Time.deltaTime / 2f);
            UpdatePropellerSpeed();
        }
        if (holding == holdType.nuke && curHeight > hoverHeight)
        {
            rb.AddForce(Vector3.up * -nukeHoverSpeed * webbedSpeedMod * Time.deltaTime * 2f);
            UpdatePropellerSpeed();
        }
        else if(holding == holdType.nuke && curHeight < hoverHeight)
        {
            rb.AddForce(Vector3.up * nukeHoverSpeed * webbedSpeedMod * Time.deltaTime);
            UpdatePropellerSpeed();
        }
        if (hm.activeEffects[33].x > 0) { foreach (PropellerSpin propeller in propellers) { propeller.speed = 0f; } }
    }
    void UpdatePropellerSpeed()
    {
        if(holding != holdType.nuke)
        {
            if (curHeight < hoverHeight)
            {
                foreach (PropellerSpin propeller in propellers) { propeller.speed = 1800f; }
            }
            else
            {
                foreach (PropellerSpin propeller in propellers) { propeller.speed = 800f; }
            }
        }
        else
        {
            if (curHeight < hoverHeight)
            {
                foreach (PropellerSpin propeller in propellers) { propeller.speed = 2600f; }
            }
            else
            {
                foreach (PropellerSpin propeller in propellers) { propeller.speed = 2600f; }
            }
        }

    }
    private void OnDisable()
    {
        foreach (PropellerSpin propeller in propellers) { propeller.speed = 0f; }
    }
    void AttemptShoot()
    {
        if(holding == holdType.uzi && CanShootUzi())
        {
            if(uCooldownTimer <= 0)
            {
                if(bulShot <= UziBustAmt)
                {
                    if(uBurstTimer <= 0)
                    {
                        Shoot();
                        uBurstTimer = uziBurstCooldown;
                    } else { uBurstTimer -= Time.deltaTime; }
                } else { uCooldownTimer = uziCooldown; bulShot = 0; if (jammed) { hm.activeEffects[30] -= new Vector4(1, 0, 0, 0); } }
            } else { uCooldownTimer -= Time.deltaTime; }
        }
        else if(holding == holdType.grenade)
        {
            if (gCooldownTimer <= 0)
            {
                if (greShot <= greBurstAmt)
                {
                    if (gBurstTimer <= 0)
                    {
                        Shoot();
                        gBurstTimer = greBurstCooldown;
                    } else { gBurstTimer -= Time.deltaTime; }
                } else { gCooldownTimer = greCooldown; greShot = 0; if (jammed) { hm.activeEffects[30] -= new Vector4(1, 0, 0, 0); } }
            } else { gCooldownTimer -= Time.deltaTime; }
        }
        else if (holding == holdType.crab)
        {
            if (cCooldownTimer <= 0)
            {
                Shoot();
                cCooldownTimer = crabCooldown;
                if (jammed) { hm.activeEffects[30] -= new Vector4(1, 0, 0, 0); }
            }
            else { cCooldownTimer -= Time.deltaTime; }
        }
    }
    bool CanShootUzi()
    {
        Ray ray = new Ray(firePointUzi.transform.position, player.transform.position - firePointUzi.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 75))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                return true;
            }
        }

        return false;
    }
    void Shoot()
    {
        if (holding == holdType.uzi)
        {
            gunUzi.GetComponent<Animator>().speed = 1 / (uziBurstCooldown / 2f);
            gunUzi.GetComponent<Animator>().SetTrigger("shoot");
            if (jammed) { jammedUzi.Play(); return; }
            GameObject uziBul = Instantiate(uziBullet, firePointUzi.position, firePointUzi.rotation);
            uziBul.transform.LookAt(player.transform.position + player.GetComponent<Rigidbody>().velocity / 3f);
            uziBul.transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * uziAcc);
            uziBul.GetComponent<EnemyBullet>().SetStats(3 * hm.baseDamage * hm.difficultyScale * hm.difficultyStatScaling, hm);
            uziBul.GetComponent<Rigidbody>().AddForce(uziBul.transform.forward * uziBulSpeed, ForceMode.Impulse);
            bulShot++;
            hm.PlaySound(1, false, true);
        }
        else if (holding == holdType.grenade && curAmt < maxAtOnce)
        {
            gunGrenade.GetComponent<Animator>().speed = 1 / (greBurstCooldown / 2f);
            gunGrenade.GetComponent<Animator>().SetTrigger("shoot");
            if (jammed) { jammedGrenade.Play(); return; }
            GameObject spawned = Instantiate(grenade, firePointGrenade.position, firePointGrenade.rotation);
            spawned.GetComponent<Rigidbody>().AddForce(firePointGrenade.forward * greLaunchSpeed, ForceMode.Impulse);
            spawned.GetComponent<EnemyHealthManager>().refundPoints = false; ;
            greShot++; activeGernades.Add(spawned.GetComponent<EnemyHealthManager>());
            hm.PlaySound(0, false, true);
        }
        else if (holding == holdType.crab)
        {
            if (jammed) { jammedCrab.Play(); return; }
            GameObject spawnedGlob = Instantiate(crabBullet, firePointCrab.position, firePointCrab.rotation);
            spawnedGlob.GetComponent<CrateCrabGlob>().damage = 6 * hm.baseDamage * hm.difficultyScale * hm.difficultyStatScaling;
            spawnedGlob.GetComponent<CrateCrabGlob>().ehm = hm;
            spawnedGlob.GetComponent<CrateCrabGlob>().lifeTimeTimer = Random.Range(10f, 20f);
            spawnedGlob.GetComponent<Rigidbody>().AddForce(transform.forward * crabBulSpeed, ForceMode.Impulse);
        }
    }
    void MoveToTarget(Vector3 target, float desDistance)
    {
        if(holding == holdType.nuke) { speed *= nukeSpeed; }

        float webbedSpeedMod = 1f;
        if (hm.activeEffects[39].x > 0) { webbedSpeedMod = 1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136])); }

        Debug.DrawRay(transform.position, (target - transform.position).normalized * 5f, Color.red);

        if (Vector3.Distance(target, transform.position) > desDistance)
        {
            rb.AddForce((target - transform.position).normalized * speed * webbedSpeedMod * Time.deltaTime);
            if (Vector3.Distance(target, transform.position + rb.velocity) > Vector3.Distance(target, transform.position))
            {
                rb.AddForce((target - transform.position).normalized * speed * webbedSpeedMod * Time.deltaTime);
            }
        }
        else
        {
            rb.AddForce((target - transform.position).normalized * -speed * webbedSpeedMod * Time.deltaTime);
        }
        
        if (rb.velocity.magnitude > 3f) { transform.LookAt(transform.position + rb.velocity); }
        if (holding == holdType.nuke) { speed /= nukeSpeed; }
    }
    void DistanceToGround()
    {
        hoverRay = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(hoverRay, out RaycastHit hit, 200f,1))
        {
            curHeight = hit.distance;
        }
    }
    EnemyHealthManager FindActiveWalker()
    {
        List<EnemyHealthManager> ehms = new List<EnemyHealthManager>();
        foreach(EnemyHealthManager ehm in hm.gdm.activeEhms)
        {
            if(ehm.data != null && ehm.data.type == Spawnable.Type.walker)
            {
                ehms.Add(ehm);
            }
        }
        if(ehms.Count > 0)
        {
            return ehms[Random.Range(0, ehms.Count)];
        }
        else
        {
            return null;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(holding == holdType.nuke && rb.velocity.magnitude > 15)
        {
            GameObject spawnedNuke = Instantiate(nuke);
            spawnedNuke.transform.position = transform.position;
            spawnedNuke.transform.rotation = transform.rotation;
            spawnedNuke.GetComponent<NuclearExplosion>().damage = 125 * hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
            Destroy(gameObject);
        }
    }
}
