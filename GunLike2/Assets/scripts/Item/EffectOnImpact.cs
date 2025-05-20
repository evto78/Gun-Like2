using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnImpact : MonoBehaviour
{
    public GameObject effect;
    public GameObject shockwave;
    public bool makeWave;
    bool spawnedWave = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            effect.SetActive(true);
        }
        if (makeWave && !spawnedWave)
        {
            spawnedWave = true;
            GameObject spawnedShockwave = Instantiate(shockwave);
            spawnedShockwave.transform.position = transform.position;
            spawnedShockwave.GetComponent<Shockwave>().lifetime = 1;
            spawnedShockwave.GetComponent<Shockwave>().damage = 50f;
        }
    }
}
