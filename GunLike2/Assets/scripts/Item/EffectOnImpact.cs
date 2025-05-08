using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnImpact : MonoBehaviour
{
    public GameObject effect;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            effect.SetActive(true);
        }
    }
}
