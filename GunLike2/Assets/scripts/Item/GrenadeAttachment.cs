using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAttachment : MonoBehaviour
{
    public GameObject shockwave;
    public GameObject grenadeGas;
    public float damage;
    public bool isGas;
    private void Start()
    {
        if (isGas) { transform.localScale = Vector3.one / 0.75f; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isGas)
        {
            GameObject spawnedGas = Instantiate(grenadeGas);
            spawnedGas.transform.position = transform.position;
            Destroy(gameObject);
        }
        else
        {
            GameObject spawnedShockwave = Instantiate(shockwave);
            spawnedShockwave.transform.position = transform.position;
            spawnedShockwave.GetComponent<Shockwave>().damage = damage;
            spawnedShockwave.GetComponent<Shockwave>().lifetime = 1f;
            Destroy(gameObject);
        }
    }
}
