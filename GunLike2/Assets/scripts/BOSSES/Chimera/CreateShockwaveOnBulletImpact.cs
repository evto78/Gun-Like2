using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShockwaveOnBulletImpact : MonoBehaviour
{
    public GameObject shockwave; float shockwaveDmg; public float lifetime;
    private void Start()
    {
        shockwaveDmg = GetComponent<EnemyBullet>().dmg;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject spawnedShockwave = Instantiate(shockwave);
        spawnedShockwave.transform.position = transform.position;
        spawnedShockwave.GetComponent<Shockwave>().damage = shockwaveDmg;
        spawnedShockwave.GetComponent<Shockwave>().lifetime = lifetime;
    }
}
