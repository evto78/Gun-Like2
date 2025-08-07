using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWPBossTurret : MonoBehaviour
{
    Animator anim; public GameObject gun;
    LineRenderer lr; public GameObject firepoint; public GameObject awpBullet;  public ParticleSystem awpMPS;
    float waitTimer; Transform player; bool shooting; Vector3 trackingPoint; float trackingTimer;
    float tarY;
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        player = GameObject.Find("Player").transform;
        anim = gameObject.GetComponentInChildren<Animator>();
        tarY = Random.Range(35, 80);
        transform.position = new Vector3(transform.position.x, -20f, transform.position.z);
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, tarY, transform.position.z), Time.deltaTime/3f);

        if(trackingTimer > 0)
        {
            trackingTimer -= Time.deltaTime;
            trackingPoint = player.position;
            gun.transform.LookAt(trackingPoint);
            transform.LookAt(player.position + (Vector3.up * (transform.position.y - player.position.y)));
        }

        lr.SetPosition(0, firepoint.transform.position);
        lr.SetPosition(1, trackingPoint);

        waitTimer -= Time.deltaTime;
        if(waitTimer < 0 && shooting)
        {
            shooting = false;
            Shoot();
        }

        lr.enabled = shooting;
    }
    public void PrepareShoot(float waitTime, float tracking)
    {
        shooting = true;
        waitTimer = waitTime;
        trackingTimer = tracking;
    }
    void Shoot()
    {
        anim.SetTrigger("Fire");
        EnemyBullet eb; GameObject spawnedBul;
        awpMPS.Play();
        spawnedBul = Instantiate(awpBullet, firepoint.transform.position, firepoint.transform.rotation);
        eb = spawnedBul.GetComponent<EnemyBullet>();
        eb.transform.LookAt(trackingPoint);
        eb.gameObject.GetComponent<Rigidbody>().AddForce(eb.transform.forward * 6f, ForceMode.Impulse);
        eb.SetStats(70, null);
    }
}
