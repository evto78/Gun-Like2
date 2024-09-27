using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGrowthScript : MonoBehaviour
{
	float explosionTimer;
	float damage;

	public Collider myCollider;

	void Awake()
	{
		myCollider.enabled = false;
	}

	public void Explode(float numberOfExplosiveGrowths, float damageTaken)
	{
		damage = (damageTaken / 3f + 1f) * (numberOfExplosiveGrowths / 5f + 1f);
		explosionTimer = 0.5f;

		transform.localScale = new Vector3((numberOfExplosiveGrowths * 2f + 14f), (numberOfExplosiveGrowths * 2f + 14f), (numberOfExplosiveGrowths * 2f + 14f));
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform.position, "self");
		}
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponentInParent<HealthManager>().TakeDamage(damage, true);
		}
	}

	private void Update()
	{
		explosionTimer -= Time.deltaTime;
		if (explosionTimer <= 0f)
		{
			myCollider.enabled = true;
			Destroy(gameObject, 0.5f);
		}
	}
}