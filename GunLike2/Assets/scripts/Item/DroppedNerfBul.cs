using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedNerfBul : MonoBehaviour
{
    public Transform player;
    public GunScript firedFrom;
    public string whatHandThisComesFrom;
    public Rigidbody rb;
    float lifeTime = 0f;
    void Update()
    {
        lifeTime += Time.deltaTime;
        if (rb.velocity.magnitude > 0.4f) { transform.rotation = Quaternion.LookRotation(rb.velocity); }
        else if(lifeTime>0.5f){ rb.velocity /= 1.5f * (1f + Time.deltaTime); }
        if (Vector3.Distance(player.position, transform.position) < 2.5f)
        {
            if (whatHandThisComesFrom == "left") { firedFrom.manager.leftHand.transform.GetChild(0).gameObject.SendMessage("addBullet", SendMessageOptions.DontRequireReceiver); }
            if (whatHandThisComesFrom == "right") { firedFrom.manager.rightHand.transform.GetChild(0).gameObject.SendMessage("addBullet", SendMessageOptions.DontRequireReceiver); }
            Destroy(gameObject);
        }
    }
}
