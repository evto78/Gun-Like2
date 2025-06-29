using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHitbox : MonoBehaviour
{
    public float damage;
    public bool doesKnockback;
    public float knockback;
    public float delayedActivationTime;
    float delayTimer;
    public float activatedTime;
    float activeTimer;
    bool collided;

    public Collider explosionCollider;

    void Start()
    {
        collided = false;
        delayTimer = delayedActivationTime;
        activeTimer = activatedTime;
        explosionCollider.enabled = false;
    }

    void Update()
    {
        delayTimer -= Time.deltaTime;
        if (delayTimer <= 0) { activeTimer -= Time.deltaTime; explosionCollider.enabled = true; }
        if(activeTimer <= 0) { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !collided)
        {
            collided = true;
            other.gameObject.GetComponent<HealthManager>().TakeDamage(damage, false, null);
            if (doesKnockback)
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(((other.transform.position - transform.position).normalized + Vector3.up * 0.1f) * knockback, ForceMode.Impulse);
            }
        }
        if(other.transform.parent.gameObject.TryGetComponent<GrenadeBrain>(out GrenadeBrain grenade))
        {
            grenade.Ticking = true;
            other.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(((other.transform.position - transform.position).normalized + Vector3.up * 0.1f) * knockback, ForceMode.Impulse);
        }

    }
}
