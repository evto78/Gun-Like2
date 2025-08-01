using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraBrain : MonoBehaviour
{
    public enum phaseTypes { a, ba, bb, c} public phaseTypes phase;
    public float awpTimer; bool awpActive;
    public float akTimer; bool akActive;
    public float uziTimer; bool uziActive;
    EnemyHealthManager ehm; public float percentage;
    public float uziHitDmg = 0.5f; public float akHitDmg = 0;
    float masterDmg; List<int> milestones;
    TrashOrb trashOrb;
    public GameObject awpHead; Animator awpAnim;
    public GameObject ak47Head; Animator ak47Anim;
    public GameObject uziHead; Animator uziAnim;

    public GameObject droppedAK; float dropAkTimer;
    public GameObject droppedUZI; float dropUziTimer;
    void Start()
    {
        milestones = new List<int>();
        milestones.Add(90);
        milestones.Add(75);
        milestones.Add(45);
        milestones.Add(30);
        milestones.Add(10);
        milestones.Add(-1);
        ehm = GetComponent<EnemyHealthManager>();
        awpTimer = 12f; akTimer = 8f; uziTimer = 4f; phase = phaseTypes.a;
        masterDmg = ehm.baseDamage * ehm.difficultyScale * ehm.gdm.difficulty;
        trashOrb = GetComponentInChildren<TrashOrb>();

        awpAnim = awpHead.GetComponentInChildren<Animator>();
        ak47Anim = ak47Head.GetComponentInChildren<Animator>();
        uziAnim = uziHead.GetComponentInChildren<Animator>();

        ehm.playerHM.uiMan.bossHealthBars[0].gameObject.SetActive(true);
        ehm.playerHM.uiMan.bossHealthBars[0].ehm = ehm;
    }
    private void OnDestroy()
    {
        if(ehm.playerHM.uiMan.bossHealthBars[0] != null)
        {
            ehm.playerHM.uiMan.bossHealthBars[0].gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (ehm.playerHM.uiMan.bossHealthBars[0] != null)
        {
            ehm.playerHM.uiMan.bossHealthBars[0].gameObject.SetActive(false);
        }
    }
    void Update()
    {
        percentage = (ehm.curHp / ehm.maxHp) * 100f;
        TimerManagement();
        switch (phase) //make sure phase change milestones are correct
        {
            case (phaseTypes.a):
                if (percentage <= 75 && uziHitDmg >= akHitDmg) { phase = phaseTypes.ba; uziAnim.SetTrigger("Fall"); dropUziTimer = 0.45f; awpAnim.SetTrigger("TakeUzi"); } // Uzi Falls off
                else if (percentage <= 75 && akHitDmg > uziHitDmg) { phase = phaseTypes.bb; ak47Anim.SetTrigger("Fall"); dropAkTimer = 0.45f; awpAnim.SetTrigger("TakeAk"); } // Ak Falls off
                break;
            case (phaseTypes.ba):

                if (percentage <= 30) { phase = phaseTypes.c; ak47Anim.SetTrigger("Fall"); dropAkTimer = 0.45f; awpAnim.SetTrigger("TakeAk"); }
                break;
            case (phaseTypes.bb):

                if (percentage <= 30) { phase = phaseTypes.c; uziAnim.SetTrigger("Fall"); dropUziTimer = 0.45f; awpAnim.SetTrigger("TakeUzi"); }
                break;
            case (phaseTypes.c):
                break;
        }
        if(milestones.Count > 1 && percentage < milestones[0]) { trashOrb.PlayHit(); milestones.RemoveAt(0); }
        if(dropAkTimer > 0)
        {
            dropAkTimer -= Time.deltaTime;
            if(dropAkTimer <= 0) { Instantiate(droppedAK, ak47Anim.transform.position, ak47Anim.transform.rotation); ak47Head.SetActive(false); }
        }
        if (dropUziTimer > 0)
        {
            dropUziTimer -= Time.deltaTime;
            if (dropUziTimer <= 0) { Instantiate(droppedUZI, uziAnim.transform.position, uziAnim.transform.rotation); uziHead.SetActive(false); }
        }
    }
    void TimerManagement()
    {
        switch (phase) //make sure timer is correct
        {
            case (phaseTypes.a):
                if (awpActive) { awpTimer = 12f; awpActive = false; } awpTimer -= Time.deltaTime; if (awpTimer <= 0) { awpActive = true; }
                if (akActive) { akTimer = 8f; akActive = false; } akTimer -= Time.deltaTime; if (akTimer <= 0) { akActive = true; }
                if (uziActive) { uziTimer = 4f; uziActive = false; } uziTimer -= Time.deltaTime; if (uziTimer <= 0) { uziActive = true; }
                break;
            case (phaseTypes.ba):
                if (awpActive) { awpTimer = 6f; awpActive = false; } awpTimer -= Time.deltaTime; if (awpTimer <= 0) { awpActive = true; }
                if (akActive) { akTimer = 4f; akActive = false; } akTimer -= Time.deltaTime; if (akTimer <= 0) { akActive = true; }
                break;
            case (phaseTypes.bb):
                if (awpActive) { awpTimer = 6f; awpActive = false; } awpTimer -= Time.deltaTime; if (awpTimer <= 0) { awpActive = true; }
                if (uziActive) { uziTimer = 2f; uziActive = false; } uziTimer -= Time.deltaTime; if (uziTimer <= 0) { uziActive = true; }
                break;
            case (phaseTypes.c):
                if (awpActive) { awpTimer = 4f; awpActive = false; } awpTimer -= Time.deltaTime; if (awpTimer <= 0) { awpActive = true; }
                break;
        }
    }
    public void AWPHIT()
    {

    }
    public void AK47HIT()
    {
        akHitDmg += 1;
    }
    public void UZIHIT()
    {
        uziHitDmg += 1;
    }
}
