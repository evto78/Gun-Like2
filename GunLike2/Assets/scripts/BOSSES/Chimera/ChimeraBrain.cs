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
    public float uziHitDmg; public float akHitDmg;
    float masterDmg; int milestone;
    TrashOrb trashOrb;
    void Start()
    {
        milestone = 90;
        ehm = GetComponent<EnemyHealthManager>();
        awpTimer = 12f; akTimer = 8f; uziTimer = 4f; phase = phaseTypes.a;
        masterDmg = ehm.baseDamage * ehm.difficultyScale * ehm.gdm.difficulty;
        trashOrb = GetComponentInChildren<TrashOrb>();
    }
    void Update()
    {
        percentage = (ehm.curHp / ehm.maxHp) * 100f;
        TimerManagement();
        switch (phase) //make sure phase change milestones are correct
        {
            case (phaseTypes.a):

                if (percentage <= 60 && uziHitDmg > akHitDmg) { phase = phaseTypes.ba; } // Uzi Falls off
                if (percentage <= 60 && akHitDmg > uziHitDmg) { phase = phaseTypes.bb; } // Ak Falls off
                break;
            case (phaseTypes.ba):

                if (percentage <= 40) { phase = phaseTypes.c; }
                break;
            case (phaseTypes.bb):

                if (percentage <= 40) { phase = phaseTypes.c; }
                break;
            case (phaseTypes.c):
                break;
        }
        if(percentage < milestone) { trashOrb.PlayHit(); milestone -= 10; }
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
}
