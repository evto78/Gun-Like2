using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatedKnifeBulletScript : BulletScript
{
    float lifetime = 0;

    private void Awake()
    {
        hm = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
        pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItem>();

        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30f);
        collided = false;

        mesh.transform.localEulerAngles = mesh.transform.localEulerAngles + Vector3.forward * (Random.Range(0f,360f));
    }

    public override void DetectCollision(Vector3 force)
    {
        Vector3.Distance(myPos, (myPos + force * Time.fixedDeltaTime));
        myPos = transform.position;
        if (Physics.BoxCast(myPos, gameObject.GetComponentInChildren<BoxCollider>().size, force, out RaycastHit hit, transform.rotation, Vector3.Distance(myPos, (myPos + force * Time.fixedDeltaTime))))
        {
            //transform.position = hit.point - transform.forward;
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "EnemyWeakPoint" || hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Untagged" || hit.collider.gameObject.layer == 0) { RunOnCollide(hit.collider.gameObject); }
        }
    }

    private void LateUpdate()
    {
        lifetime += Time.deltaTime;
        if (lifetime > 0.1f && !collided)
        {
            pierce = 0;
            RunOnCollide(gameObject);
        }
    }
}
