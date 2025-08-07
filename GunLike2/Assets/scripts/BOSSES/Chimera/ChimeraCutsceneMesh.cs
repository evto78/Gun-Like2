using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraCutsceneMesh : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 startRot;
    public float introTimer; Animator anim; public GameObject bossPrefab; bool spawned; GameObject spawnedBoss; Vector3 lastBossPos;
    public float outroTimer;
    void Start()
    {
        transform.position = startPos;
        transform.eulerAngles = startRot;
        spawned = false;
    }
    void Update()
    {
        introTimer -= Time.deltaTime;
        if(introTimer < 0 && !spawned) { transform.GetChild(0).gameObject.SetActive(false); spawnedBoss = Instantiate(bossPrefab, transform.position, transform.rotation); spawned = true; }
        if (spawned && spawnedBoss == null) { transform.position = lastBossPos; transform.GetChild(0).gameObject.SetActive(true); anim.SetTrigger("Outro"); }
        else if (spawned) { lastBossPos = spawnedBoss.transform.position; }
    }
}
