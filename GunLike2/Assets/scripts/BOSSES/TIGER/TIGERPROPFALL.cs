using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIGERPROPFALL : MonoBehaviour
{
    bool collided = false;
    Vector3 startPos;
    Rigidbody rb;
    public List<Transform> breakOffObjects;
    private void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<TIGERBrain>(out TIGERBrain tigerBrain) && !collided)
        {
            CollideNow(collision.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<NuclearExplosion>(out NuclearExplosion nuke)) { CollideNow(other.transform.position); }
    }
    public void CollideNow(Vector3 source)
    {
        if (collided) { return; }
        collided = true;
        rb.constraints = new RigidbodyConstraints();
        rb.AddForce(Vector3.up * 45f, ForceMode.Impulse);
        rb.AddForce((transform.position - source).normalized * 100f, ForceMode.Impulse);
        foreach (Transform t in breakOffObjects)
        {
            t.parent = null;
            Rigidbody newRB = t.gameObject.AddComponent<Rigidbody>();
            newRB.AddForce((((transform.position - t.position).normalized + Vector3.up) * 60f) + new Vector3(Random.Range(-40f, 40f), 10f, Random.Range(-40f, 40f)), ForceMode.Impulse);
            newRB.AddTorque(new Vector3(Random.Range(-45f, 45f), Random.Range(-45f, 45f), Random.Range(-45f, 45f)), ForceMode.Impulse);
            Destroy(t.gameObject, 10f);
        }
        Destroy(gameObject, 10f);
    }
    void Update()
    {
        if (collided) 
        {
            foreach (Transform t in breakOffObjects) 
            {
                if(t != null)
                {
                    if (t.localScale.x <= 0.01f || t.localScale.y <= 0.01f || t.localScale.z <= 0.01f) { Destroy(t.gameObject); }
                    else { t.localScale /= 1f + (4f * Time.deltaTime); }
                }
            }
            if (transform.localScale.x <= 0.01f || transform.localScale.y <= 0.01f || transform.localScale.z <= 0.01f) { Destroy(transform.gameObject); }
            else { transform.localScale /= 1f + Time.deltaTime; }
        }
        else
        {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }
    }
}
