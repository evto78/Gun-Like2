using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnedPapers : MonoBehaviour
{
    public float damage;
    public GameObject particles;
    Collider mycollider;
    public List<GameObject> collidedWith = new List<GameObject>();
    private void Awake()
    {
        mycollider = GetComponent<Collider>();
        mycollider.enabled = false;
    }
    private void Start()
    {
        mycollider.enabled = true;
        Destroy(gameObject, 0.1f);
        particles.transform.parent = null;
        Destroy(particles, 0.3f);
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (!collidedWith.Contains(other.gameObject))
        {
            if (other.gameObject.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm))
            {
                ehm.QueStandardDamage(damage / 2f);
            }
            else if (other.transform.parent != null && other.transform.parent.gameObject.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm2))
            {
                if (!collidedWith.Contains(other.transform.parent.gameObject))
                {
                    ehm2.QueStandardDamage(damage / 2f);
                }
            }

            collidedWith.Add(other.gameObject);
        }
    }
}
