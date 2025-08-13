using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TelemData
{
    public string eventData;
    public string eventTime;

    public List<int> leftInv;
    public List<int> rightInv;

    public string leftGun;
    public string rightGun;

    public int roomNum;
    public float difficulty;
    public int selectedDifficulty;
    public float timeElapsed;

    public int currentCash;

    public string sessionNum;
    public string usr;

    public string mostRecentSourceOfDmg;
}
