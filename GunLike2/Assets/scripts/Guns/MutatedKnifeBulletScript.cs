using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatedKnifeBulletScript : BulletScript
{
    float lifetimeKnife = 0;
    private void Awake()
    {
        mesh.transform.localEulerAngles = mesh.transform.localEulerAngles + Vector3.forward * (Random.Range(0f, 360f));
    }

    public override void DetectCollision(Vector3 force)
    {
        if (collided) { return; }
        //ONLY CHANGE IS BOXCAST INSTEAD OF RAYCAST
        myPos = transform.position;
        if (Physics.BoxCast(myPos, gameObject.GetComponentInChildren<BoxCollider>().size, force, out RaycastHit hit, transform.rotation, Vector3.Distance(myPos, (myPos + force * Time.fixedDeltaTime))))
        {
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "EnemyWeakPoint" || hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Untagged" || hit.collider.gameObject.layer == 0) { RunOnCollide(hit.collider.gameObject, hit); }
        }
    }

    private void LateUpdate()
    {
        lifetimeKnife += Time.deltaTime;
        if (lifetimeKnife > 0.1f && !collided)
        {
            pierce = 0;
            RunOnCollide(gameObject, new RaycastHit());
        }
    }
}
