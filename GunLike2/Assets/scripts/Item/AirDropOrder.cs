using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropOrder : MonoBehaviour
{
    public GameObject airdrop; Animator anim; Light l;
    void Start()
    {
        anim = GetComponent<Animator>(); l = GetComponentInChildren<Light>();
    }
    public void Activate()
    {
        anim.SetTrigger("Up");
        Instantiate(airdrop);
        Destroy(l.gameObject, 2f);
    }

}
