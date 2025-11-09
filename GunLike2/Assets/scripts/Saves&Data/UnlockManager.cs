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
    
    void Start()
    {
        gdm = GetComponent<GameDataManager>();
        saveFRW = gdm.instance;
        unlockInfo = saveFRW.data.UnlockInfo;

        pi = gdm.pi;
        phm = pi.healthManager;
        mvt = pi.playerMvt;
    }
    public void UnlockItem(int id) { if (unlockInfo[id].unlockProgress >= 1) { return; } unlockInfo[id].unlockProgress = 1; pi.TriggerUnlock(id); }
    public void AddUnlockProgress(int id, float progress) { if (unlockInfo[id].unlockProgress >= 1) { return; } unlockInfo[id].unlockProgress += progress; if (unlockInfo[id].unlockProgress >= 1) { UnlockItem(id); } }
    void Update()
    {
        CheckEveryFrame();
    }
    void CheckEveryFrame()
    {
        if (mvt.rb.velocity.magnitude >= 40f && mvt.sliding) { UnlockItem(3); } // Butt-er (3)
        if (pi.leftItems[188] + pi.rightItems[188] > 0) { UnlockItem(5); } // AircraftGradeMetal (5)
        if (pi.leftMutatedItemCount + pi.rightMutatedItemCount >= 5) { UnlockItem(14); } // MutatedCell (14)
        if (phm.statusEffectsActive >= 5) { UnlockItem(17); } // OrganicGumballMachine (17)
    }
}
