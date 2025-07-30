using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropScript : MonoBehaviour
{
    Transform player;
    public float initialDistance;
    public float speed;
    float distanceTraveled;
    public float dropInterval;
    float intervalTimer;
    public int dropCount;
    int dropsDropped;

    LevelBuilder lb;

    public GameObject drop;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        transform.localEulerAngles = Vector3.up * Random.Range(0, 360);
        transform.position = player.position + (Vector3.up * 300f) + (transform.forward * -initialDistance);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        lb = GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        intervalTimer -= Time.deltaTime;
        transform.position += transform.forward * Time.deltaTime * speed;
        distanceTraveled += Time.deltaTime * speed;

        if (distanceTraveled > initialDistance / 1.5f && dropsDropped < dropCount)
        {
            if(intervalTimer <= 0)
            {
                intervalTimer = dropInterval;
                dropsDropped++;

                GameObject spawnedDrop = Instantiate(drop);
                spawnedDrop.transform.position = transform.position;
                spawnedDrop.transform.position -= Vector3.up * -10f;
                spawnedDrop.transform.position -= Vector3.forward * Random.Range(-2f, 2f);
                spawnedDrop.transform.position -= Vector3.right * Random.Range(-2f, 2f);

                spawnedDrop.transform.localEulerAngles = new Vector3(-30f, Random.Range(0f,360f), 0f);
                lb.placed.Add(spawnedDrop);
            }
        }

        if(distanceTraveled > initialDistance * 2f)
        {
            Destroy(gameObject);
        }

    }
}
