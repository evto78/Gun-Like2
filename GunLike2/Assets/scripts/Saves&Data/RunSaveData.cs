using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSaveData
{
    public int roomNumber;
    public int selectedDifficulty;
    public float currentDifficulty;
    public List<int> leftInv;
    public List<int> rightInv;
    public int leftGun;
    public int rightGun;
    public float timeElapsed;
    public float unpausedTimeElapsed;
    public List<int> mutationRules;
    public int cash;
    public int tickets;
    public Random.State randomnessSeed;
    public int appleBuff;
    public float fortifyBuff;
    public float sunflowerDebuff;

    public string runName;
    public string runCreationDate;

    public void InitializeData()
    {
        runName = "NULL RUN";
        runCreationDate = System.DateTime.Now.ToString("U");

        roomNumber = 0;
        selectedDifficulty = 1;
        currentDifficulty = 1f;
        leftInv = new List<int>();
        rightInv = new List<int>();
        leftGun = 0;
        rightGun = 0;
        timeElapsed = 0;
        unpausedTimeElapsed = 0;
        mutationRules = new List<int>();
        cash = 0;
        tickets = 0;
        randomnessSeed = Random.state;

        appleBuff = 0;
        fortifyBuff = 0;
        sunflowerDebuff = 0;
    }
}
