using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashOrb : MonoBehaviour
{
    public Transform hitPS;
    public void PlayHit()
    {
        for(int i = 0; i < hitPS.childCount; i++)
        {
            hitPS.GetChild(i).GetComponent<ParticleSystem>().Play();
        }

    }
}
