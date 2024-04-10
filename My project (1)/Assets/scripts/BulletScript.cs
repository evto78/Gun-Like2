using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody rb;
    public ParticleSystem hitParticle;
    public GameObject mesh;

    bool collided = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided)
        {
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;
            hitParticle.Play();
            Destroy(mesh);
            collided = true;
        }
        
    }

    private void FixedUpdate()
    {
        if (rb.freezeRotation)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
