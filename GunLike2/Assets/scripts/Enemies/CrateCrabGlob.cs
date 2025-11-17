using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateCrabGlob : MonoBehaviour
{
    Rigidbody rb; public float speed; public float damage; Transform player; public EnemyHealthManager ehm; public float gravity; public float turnSpeed;
    float spinamount; public GameObject hitEffect; public float lifeTimeTimer; float initialTimer; public GameObject pool; public float spawnSpeed; float spawnTimer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = ehm.playerHM.transform;
        spinamount = 0f; lifeTimeTimer = 10f;
        initialTimer = lifeTimeTimer;
        spawnTimer = spawnSpeed;
    }
    void Update()
    {
        rb.AddForce((player.position-transform.position).normalized * speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, player.position) < Vector3.Distance(transform.position + rb.velocity, player.position))
        {
            rb.velocity *= 1f - Time.deltaTime;
            rb.AddForce((player.position - transform.position).normalized * speed * 2f * Time.deltaTime);
        }
        rb.AddForce(Vector3.up * -gravity * Time.deltaTime);
        if (rb.velocity.magnitude != 0) { transform.rotation = Quaternion.LookRotation(rb.velocity); }
        spinamount += turnSpeed * Time.deltaTime;
        transform.Rotate(transform.forward * spinamount);
        if(spinamount > 360) { spinamount -= 360; }
        if(spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            GameObject spawnedPool = Instantiate(pool, transform.position - Vector3.up, Quaternion.LookRotation(Vector3.forward,Vector3.up));
            spawnedPool.GetComponent<LavaFloor>().damage = damage;
            spawnedPool.GetComponent<LavaFloor>().isInfinite = false;
            spawnedPool.GetComponent<LavaFloor>().lifetime = Random.Range(10f,20f);
            spawnTimer = spawnSpeed;
        }
        if(lifeTimeTimer > 0)
        {
            lifeTimeTimer -= Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Clamp(((lifeTimeTimer+(initialTimer/2)) / initialTimer), 0, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
            hitEffect.SetActive(true);
            hitEffect.transform.SetParent(null);
            Destroy(hitEffect, 3f);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ehm.playerHM.TakeDamage(damage, false, ehm, ehm.data.enemyName, ehm.transform);
            hitEffect.SetActive(true);
            hitEffect.transform.SetParent(null);
            Destroy(hitEffect, 3f);
            Destroy(gameObject);
        }
    }
}
