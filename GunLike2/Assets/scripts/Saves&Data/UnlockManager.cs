using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    SaveFileReadWrite saveFRW;
    GameDataManager gdm;
    PlayerItem pi;
    HealthManager phm;
    NEWPlayerMovement mvt;
    List<SaveFileReadWrite.UnlockInformation> unlockInfo = new List<SaveFileReadWrite.UnlockInformation>();

    bool startNewRoutine = true;
    
    void Start()
    {
        gdm = GetComponent<GameDataManager>();
        saveFRW = gdm.instance;
        unlockInfo = saveFRW.data.UnlockInfo;

        pi = gdm.pi;
        phm = gdm.phm;
        mvt = gdm.phm.playerMvt;

        StartCoroutine(CheckForUnlocksEfficent());
    }
    public void UnlockItem(int id) { if (unlockInfo[id].unlockProgress >= 1) { return; } unlockInfo[id].unlockProgress = 1; pi.TriggerUnlock(id); }
    public void AddUnlockProgress(int id, float progress) 
    { if (unlockInfo[id].unlockProgress >= 1) { return; } unlockInfo[id].unlockProgress += progress; if (unlockInfo[id].unlockProgress >= 1) { UnlockItem(id); } }
    public void SetUnlockProgressNOLOSS(int id, float progress) 
    { if (unlockInfo[id].unlockProgress >= 1 || unlockInfo[id].unlockProgress > progress) { return; } else { unlockInfo[id].unlockProgress = progress; if (unlockInfo[id].unlockProgress >= 1) { UnlockItem(id); } } }
    void Update()
    {
        if (startNewRoutine) { StartCoroutine(CheckForUnlocksEfficent()); }
    }
    IEnumerator CheckForUnlocksEfficent()
    {
        startNewRoutine = false;
        //Try to limit the amount of "If"s each frame. If something needs to be checked more often, add it to more frames.

        // --- FRAME 1 --- 
        CheckQuickUnlocks();
        CheckMedUnlocks(3);
        CheckSlowUnlocks(0);
        yield return new WaitForEndOfFrame();

        // --- FRAME 2 ---
        CheckMedUnlocks(0);
        CheckSlowUnlocks(1);
        yield return new WaitForEndOfFrame();

        // --- FRAME 3 --- 
        CheckQuickUnlocks();
        CheckMedUnlocks(1);
        CheckSlowUnlocks(2);
        yield return new WaitForEndOfFrame();

        // --- FRAME 4 --- 
        CheckMedUnlocks(2);
        CheckSlowUnlocks(3);
        yield return new WaitForEndOfFrame();

        // --- FRAME 5 --- 
        CheckQuickUnlocks();
        CheckMedUnlocks(3);
        CheckSlowUnlocks(4);
        yield return new WaitForEndOfFrame();

        // --- FRAME 6 --- 
        CheckMedUnlocks(0);
        CheckSlowUnlocks(5);
        yield return new WaitForEndOfFrame();

        // --- FRAME 7 --- 
        CheckQuickUnlocks();
        CheckMedUnlocks(1);
        CheckSlowUnlocks(6);
        yield return new WaitForEndOfFrame();

        // --- FRAME 8 --- 
        CheckMedUnlocks(2);
        CheckSlowUnlocks(7);

        startNewRoutine = true; yield return null;
    }
    void CheckQuickUnlocks() //Checked every 2 frames, over 1 frame
    {
        if (mvt.rb.velocity.magnitude >= 60f && mvt.sliding) { UnlockItem(3); } // Butt-er (3)
    }
    void CheckMedUnlocks(int frame) //Checked every 4 frames, over 4 frames
    {
        switch(frame)
        {
            case 0:
                if (phm.statusEffectsActive >= 5) { UnlockItem(17); } // OrganicGumballMachine (17)
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    void CheckSlowUnlocks(int frame) //Checked every 8 frames, over 8 frames
    {
        switch(frame)
        {
            case 0:
                if (pi.leftItems[188] + pi.rightItems[188] > 0) { UnlockItem(5); } // AircraftGradeMetal (5)
                if (pi.leftMutatedItemCount + pi.rightMutatedItemCount >= 5) { UnlockItem(14); } // MutatedCell (14)
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
        }
    }
    public void CheckAllUnlocks()
    {
        CheckQuickUnlocks(); 
        CheckMedUnlocks(0); 
        CheckMedUnlocks(1); 
        CheckMedUnlocks(2); 
        CheckMedUnlocks(3); 
        CheckSlowUnlocks(0);
        CheckSlowUnlocks(1);
        CheckSlowUnlocks(2);
        CheckSlowUnlocks(3);
        CheckSlowUnlocks(4);
        CheckSlowUnlocks(5);
        CheckSlowUnlocks(6);
        CheckSlowUnlocks(7);
    }
    //Cheats
    public void UnlockAll()
    {
        foreach(SaveFileReadWrite.UnlockInformation unlock in unlockInfo)
        { UnlockItem(unlock.id); }
    }
    public void LockAll()
    {
        foreach (SaveFileReadWrite.UnlockInformation unlock in unlockInfo)
        { if(unlock.unlockCondition != null) { unlock.unlockProgress = 0; } }
    }
}
