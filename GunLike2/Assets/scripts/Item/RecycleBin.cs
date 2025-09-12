using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleBin : MonoBehaviour
{
    Animator anim; Transform player;
    GameDataManager gdm;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        player = gdm.phm.transform;
    }
    void LateUpdate()
    {
        if(Vector3.Distance(player.position, transform.position) < 15)
        {
            anim.SetBool("Open", true);
            gdm.phm.uiMan.recycleBinUI.gameObject.SetActive(true);
            gdm.phm.uiMan.timeSinceRecycleBinUpdate = 0;
        }
        else { anim.SetBool("Open", false); }
    }
}
