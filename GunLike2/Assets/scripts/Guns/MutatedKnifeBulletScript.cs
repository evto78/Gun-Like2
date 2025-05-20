using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatedKnifeBulletScript : BulletScript
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 0.2f);
        collided = false;
    }
}
