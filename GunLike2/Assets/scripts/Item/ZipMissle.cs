using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipMissle : MonoBehaviour
{
    Rigidbody rb;
    public EnemyHealthManager targetEhm;
    public float damage;
    public float thrust;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb.useGravity) { rb.AddForce(Vector3.up * 10f, ForceMode.Impulse); }
        if (targetEhm == null)
        {
            float tempDist = 999999f;
            foreach (GameObject ehm in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (Vector3.Distance(ehm.transform.position, transform.position) < tempDist && ehm.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm2))
                {
                    tempDist = Vector3.Distance(ehm.transform.position, transform.position);
                    targetEhm = ehm2;
                }
            }
            if(targetEhm == null) { Destroy(gameObject); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEhm == null)
        {
            float tempDist = 999999f;
            foreach (GameObject ehm in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (Vector3.Distance(ehm.transform.position, transform.position) < tempDist && ehm.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm2))
                {
                    tempDist = Vector3.Distance(ehm.transform.position, transform.position);
                    targetEhm = ehm2;
                }
            }
            if (targetEhm == null) { Destroy(gameObject); }
        }

        if (rb.velocity.magnitude > 0) { transform.rotation.SetLookRotation(rb.velocity); }

        rb.AddForce(( targetEhm.transform.position- transform.position).normalized * thrust * Time.deltaTime);
        if (rb.useGravity) { rb.AddForce(Vector3.up * Time.deltaTime * 60f); }

        if(Vector3.Distance(transform.position + rb.velocity, targetEhm.transform.position) > Vector3.Distance(transform.position, targetEhm.transform.position))
        {
            rb.AddForce((targetEhm.transform.position - transform.position).normalized * thrust * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyWeakPoint")
        {
            if (collision.transform.parent != null)
            {
                collision.transform.parent.GetComponent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform.position, "player");
            }
            else
            {
                collision.transform.GetComponent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform.position, "player");
            }
            Destroy(gameObject);
        }
    }
}
