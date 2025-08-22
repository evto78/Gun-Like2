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
    void Update()
    {
        anim.SetBool("Open", Vector3.Distance(player.position, transform.position) < 15);
    }
}
