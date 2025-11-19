using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public bool hurtPlayer;
    public float lifetime;
    public float damage;
    float lifetimeTimer;
    public List<MeshRenderer> myMeshs;

    public bool coolSpon;
    public bool fireSpon;
    public bool bleedSpon;
    public bool helpingSpon;

    public bool blinding;

    PlayerItem playerItem;

    void Start()
    {
        playerItem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItem>();

        Destroy(gameObject, lifetime/2f);
        lifetimeTimer = 0;
    }
    void Update()
    {
        lifetimeTimer += Time.deltaTime*2f;

        if(playerItem.leftItems[53] + playerItem.rightItems[53] > 0)
        {
            transform.localScale = Vector3.one * lifetimeTimer * 16f;
        }
        else
        {
            transform.localScale = Vector3.one * lifetimeTimer * 8f;
        }
        if (lifetimeTimer >= lifetime) { lifetimeTimer = lifetime - 0.01f; }
        for(int i = 0; i < myMeshs.Count; i++)
        {
            myMeshs[i].material.color = new Color(myMeshs[i].material.color.r, myMeshs[i].material.color.g, myMeshs[i].material.color.b, ((lifetime - lifetimeTimer) / lifetime) / 2f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (hurtPlayer)
        {
            if (collision.gameObject.tag == "Player")
            {
                HealthManager phm = collision.gameObject.GetComponentInParent<HealthManager>();
                if(phm == null) { return; }
                phm.TakeDamage(damage, false, null, "Shockwave", transform);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Enemy")
            {
                EnemyHealthManager ehm = collision.gameObject.GetComponentInParent<EnemyHealthManager>();
                if(ehm == null) { return; }
                ehm.TakeDamage(damage, false, HitType.ht.normal, collision.gameObject.transform.position, "self");
                if (blinding) { ehm.GiveEffect(37, playerItem.leftItems[125] + playerItem.rightItems[125]); }
                if (coolSpon) { ehm.GiveEffect(33, 1f); }
                if (fireSpon) { ehm.GiveEffect(1, 3f); }
                if (bleedSpon) { ehm.GiveEffect(0, 3f); }
                if (helpingSpon) { ehm.GiveEffect(32, 1f); }
            }
        }
    }
}
