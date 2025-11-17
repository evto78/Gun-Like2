using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UZIBoomerang : MonoBehaviour
{
    float offset; public float damage;
    public float spinSpeed; public Vector3 targetPos; Vector3 spawnPos; public Transform uziHead; public float movementSpeed; float curSpeed; float progress; bool direction;
    void Start()
    {
        offset = 0;
        spawnPos = transform.position;
        targetPos = GameObject.Find("Player").transform.position;
        curSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetPos);
        offset += spinSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right * offset);

        if (!direction)
        {
            transform.position = Vector3.Lerp(spawnPos, targetPos, progress);
            progress += curSpeed * Time.deltaTime;
            curSpeed += movementSpeed * Time.deltaTime;
            if(progress > 1) { direction = true; progress = 0; curSpeed = 0; }
        }
        else
        {
            transform.position = Vector3.Lerp(targetPos, uziHead.position, progress);
            progress += curSpeed * Time.deltaTime;
            curSpeed += movementSpeed * Time.deltaTime;
            if (progress > 1) { Destroy(gameObject); }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthManager>().TakeDamage(damage, false, null, "Chimera", transform);
        }
    }
}
