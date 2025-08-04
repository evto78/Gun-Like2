using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwpBullet : MonoBehaviour
{
    public GameObject shockwave; float shockwaveDmg;
    private void Start()
    {
        shockwaveDmg = GetComponent<EnemyBullet>().dmg;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject spawnedShockwave = Instantiate(shockwave);
        spawnedShockwave.transform.position = transform.position;
        spawnedShockwave.GetComponent<Shockwave>().damage = shockwaveDmg;
        spawnedShockwave.GetComponent<Shockwave>().lifetime = 1f;
    }
}
