using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
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
        //myMeshs.AddRange(gameObject.GetComponentsInChildren<MeshRenderer>());
    }

    // Update is called once per frame
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
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", collision.gameObject.transform.position, "self");
            if (blinding) { collision.gameObject.GetComponentInParent<EnemyHealthManager>().GiveEffect("blind", playerItem.leftItems[125]+playerItem.rightItems[125]); }
            if (coolSpon) { collision.gameObject.GetComponentInParent<EnemyHealthManager>().GiveEffect("frozen", 1f); }
            if (fireSpon) { collision.gameObject.GetComponentInParent<EnemyHealthManager>().GiveEffect("burn", 3f); }
            if (bleedSpon) { collision.gameObject.GetComponentInParent<EnemyHealthManager>().GiveEffect("bleed", 3f); }
            if (helpingSpon) { collision.gameObject.GetComponentInParent<EnemyHealthManager>().GiveEffect("stiched", 1f); }
        }
    }
}
