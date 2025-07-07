using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTurretAlly : MonoBehaviour
{
    Animator anim;
    LineRenderer laser;
    public EnemyHealthManager target;
    public float damage;
    LookAtObject lookAtGun;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 10f;
        laser = gameObject.GetComponent<LineRenderer>();
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("Active", true);
        lookAtGun = GetComponentInChildren<LookAtObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(target != null)
        {
            lookAtGun.lookAt = target.transform;
            if (timer <= 0 && anim.GetBool("Active")) { Shoot(); anim.SetBool("Active", false); }
            if (timer <= -3) { Destroy(gameObject); }
            DrawLaser();
        }
        else
        {
            anim.SetBool("Active", false);
            if (timer <= -3) { Destroy(gameObject); }
            laser.enabled = false;
        }
    }
    void Shoot()
    {
        target.TakeDamage(damage, true, HitType.ht.normal, target.transform.position, "other");
    }
    void DrawLaser()
    {
        laser.enabled = false;
        if(timer < 9f) { laser.enabled = true; }
        if(timer > 0) { laser.startWidth = (timer / 10f) / 10f; laser.endWidth = (timer / 10f) / 10f; }
        else if(timer > -0.1) { laser.startWidth = 0.2f; laser.endWidth = 0.2f; }
        else { laser.enabled = false; }
        laser.SetPosition(0, lookAtGun.transform.position);
        laser.SetPosition(1, target.transform.position);
    }
}
