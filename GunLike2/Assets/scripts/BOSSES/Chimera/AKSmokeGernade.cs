using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKSmokeGernade : MonoBehaviour
{
    public GameObject activeSmoke;
    public Collider smokeTrigger;
    float triggerActivateTimer; bool actiavted;
    private void Start()
    {
        actiavted = false;
        smokeTrigger.enabled = false;
        activeSmoke.SetActive(false);
    }
    private void Update()
    {
        if(actiavted && triggerActivateTimer < 0)
        {
            smokeTrigger.enabled = true;
        }
        triggerActivateTimer -= Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && !actiavted)
        {
            actiavted = true;
            activeSmoke.SetActive(true);
            triggerActivateTimer = 3f;
            Destroy(gameObject, 20f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthManager>().GiveEffect(28, 1);
        }
    }

}
