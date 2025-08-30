using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public float healPer;
    private void Start()
    {
        Destroy(gameObject, 60f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            HealthManager hm = other.gameObject.GetComponent<HealthManager>();
            hm.TakeDamage(-hm.maxHp * (healPer / 100f), false, null, "Chicken Coop");

            Destroy(gameObject);
        }

    }
}
