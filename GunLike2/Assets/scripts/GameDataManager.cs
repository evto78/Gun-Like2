using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public List<EnemyHealthManager> activeEhms = new List<EnemyHealthManager>();
    public float difficulty;
    public float difficultySelected;
    public float timeSpent;
    public bool gameTimerActive;
    public int roomNumber;
    HealthManager phm;
    private void Start()
    {
        phm = GameObject.Find("Player").GetComponent<HealthManager>();
        roomNumber = 0;
        timeSpent = 0;
        difficulty = Mathf.RoundToInt((difficultySelected * timeSpent / 300f) + 1f);
    }
    private void Update()
    {
        if (gameTimerActive)
        {
            timeSpent += Time.deltaTime;
        }
        difficulty = Mathf.RoundToInt((difficultySelected * timeSpent / 300f) + 1f);
    }
    //NEEDS to be called when the player goes into the next room.
    public void AdvanceToNextRoom()
    {
        roomNumber += 1;
        phm.attackedThisRoom = false;
        phm.brokenSpeakerItemDropped = false;
    }
}
