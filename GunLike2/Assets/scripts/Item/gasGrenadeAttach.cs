using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasGrenadeAttach : MonoBehaviour
{
    float timer = 15f;
    float effectTimer = 0f;
    bool effecting;
    private void Update()
    {
        effectTimer -= Time.deltaTime;
        effecting = effectTimer <= 0;
        if(effectTimer < 0) { effectTimer = 0.5f; }
        timer -= Time.deltaTime;
        if(timer < 0) { Destroy(gameObject); }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.parent != null)
        {
            if (other.transform.parent.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm))
            {
                if (effecting) { ehm.GiveEffect("gas", 1f); }
            }
        }
        else
        {
            if(other.transform.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm))
            {
                if (effecting) { ehm.GiveEffect("gas", 1f); }
            }
        }
        
    }
}
