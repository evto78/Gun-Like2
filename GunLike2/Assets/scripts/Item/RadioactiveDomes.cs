using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioactiveDomes : MonoBehaviour
{
    float explosionTimer;
    public float damage;

    SphereCollider myCollider;
    void Start()
    {
        myCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        explosionTimer += Time.deltaTime * 15f;
        myCollider.radius = explosionTimer;
        if(explosionTimer > 30f) { Destroy(gameObject); }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, HitType.ht.normal, collision.gameObject.transform.position, "self");
        }
    }
}
