using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBall : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    public float healing;
    public float speed;
    float jumpTimer;
    void Start()
    {
        player = GameObject.Find("Player");
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.AddForce((player.transform.position - transform.position).normalized * 10f * speed * Time.deltaTime);
        if(jumpTimer < 0)
        {
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            jumpTimer = Random.Range(3f, 4f);
        }
        jumpTimer -= Time.deltaTime;

        if(Vector3.Distance(player.transform.position, transform.position) < 3f)
        {
            player.GetComponent<HealthManager>().TakeDamage(-healing, false, null, "Hungry Shot", null);
            Destroy(gameObject);
        }
    }
}
