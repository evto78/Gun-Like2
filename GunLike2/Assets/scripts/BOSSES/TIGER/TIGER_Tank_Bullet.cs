using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIGER_Tank_Bullet : MonoBehaviour
{
    public GameObject explosion; float explosionDmg;
    private void Start()
    {
        explosionDmg = GetComponent<EnemyBullet>().dmg;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject spawnedShockwave = Instantiate(explosion);
        spawnedShockwave.transform.position = transform.position;
        spawnedShockwave.GetComponent<NuclearExplosion>().damage = explosionDmg;
        spawnedShockwave.GetComponent<NuclearExplosion>().lifetime = 0.25f;
        spawnedShockwave.GetComponent<NuclearExplosion>().approachRate = 4f;
        spawnedShockwave.GetComponent<NuclearExplosion>().dontLeaveDebris = true;
    }
}
