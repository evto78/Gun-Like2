using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandbagLimb : MonoBehaviour
{
    SandbagBrain sb; LineRenderer lr;
    float shrinkTimer; bool shrinking;
    // Start is called before the first frame update
    void Start()
    {
        sb = GetComponentInParent<SandbagBrain>();
        lr = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, sb.gameObject.transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(sb.dmg, false, sb.ehm);
        }
    }
}
