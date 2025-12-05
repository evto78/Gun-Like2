using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearExplosion : MonoBehaviour
{
    float explosionTimer;
    public float damage;
    public float radiasMult;
    public float lifetime;
    public float approachRate;
    bool done;
    Light brightthingy;
    public AnimationCurve lightCurve;
    public bool dontLeaveDebris;
    public GameObject debris;

    SphereCollider myCollider;
    void Start()
    {
        myCollider = GetComponent<SphereCollider>();
        brightthingy = GetComponentInChildren<Light>();

        LevelBuilder lb = GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>();
        if (lb != null) { lb.placed.Add(gameObject); }

        if (dontLeaveDebris) { Destroy(debris); }
    }

    // Update is called once per frame
    void Update()
    {
        explosionTimer += Time.deltaTime * approachRate;
        brightthingy.intensity = lightCurve.Evaluate(explosionTimer / (lifetime * approachRate)) * 60f;
        if (myCollider != null) { myCollider.radius = explosionTimer * radiasMult; }
        if (explosionTimer > lifetime * approachRate && !done) { Destroy(myCollider); Destroy(gameObject, 30f); done = true; }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyWeakPoint")
        {
            if (collision.transform.parent != null && collision.transform.parent.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm))
            {
                ehm.TakeDamage(damage, false, HitType.ht.normal, collision.gameObject.transform.position, "self");
            }
            else if (collision.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm2))
            {
                ehm2.TakeDamage(damage, false, HitType.ht.normal, collision.gameObject.transform.position, "self");
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<HealthManager>().TakeDamage(damage, false, null, "Nuclear Explosion", transform);
        }
    }
}
