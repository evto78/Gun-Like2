using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    float timer = 5f;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) { Destroy(gameObject); }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent != null)
        {
            EnemyHealthManager ehm = other.transform.GetComponentInParent<EnemyHealthManager>();
            if (ehm != null)
            {
                if (ehm.activeEffects[12].x <= 1) { ehm.GiveEffect("webbed", 1f); }
            }
        }
        else
        {
            if (other.transform.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm))
            {
                if (ehm.activeEffects[12].x <= 1) { ehm.GiveEffect("webbed", 1f); }
            }
        }

    }
}
