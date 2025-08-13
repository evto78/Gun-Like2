using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    public float damage; public float tickrate;
    float timer; public bool isInfinite; public float lifetime; float initialLifetime;
    private void Start()
    {
        timer = 1f; initialLifetime = lifetime;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (!isInfinite)
        {
            if(lifetime > 0)
            {
                lifetime -= Time.deltaTime;
                if(lifetime / initialLifetime <= 0) { Destroy(gameObject); return; }
                transform.localScale = Vector3.one * (lifetime / initialLifetime);
            }
            else { Destroy(gameObject); }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && timer <= 0)
        {
            other.gameObject.GetComponentInParent<HealthManager>().TakeDamage(damage, false, null);
            timer = tickrate;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && timer <= 0)
        {
            collision.gameObject.GetComponentInParent<HealthManager>().TakeDamage(damage, false, null);
            timer = tickrate;
        }
    }
}
