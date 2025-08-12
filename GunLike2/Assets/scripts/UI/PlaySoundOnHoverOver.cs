using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnHoverOver : Selectable
{
    LocalSoundManager lsm; MainMenuManager mainMenu;
    bool hoverLastFrame; bool isMainMenu;
    [Header("Sound")]
    public bool useCustomSoundKey;
    public int soundKey;
    public bool useCustomSelectSoundKey;
    public int selectsoundKey;
    protected override void Start()
    {
        base.Start();
        if (lsm != null) { return; }
        GameObject lsmHost;
        lsmHost = GameObject.FindGameObjectWithTag("gdm");
        if(lsmHost != null) { lsm = lsmHost.GetComponent<GameDataManager>().phm.lsm; isMainMenu = false; }
        if (lsm == null) { mainMenu = GameObject.Find("Main Menu Manager").GetComponent<MainMenuManager>(); isMainMenu = true; }
    }
    void Update()
    {
        if (IsHighlighted()) 
        { if (!hoverLastFrame) {
                HoverSound();
        } hoverLastFrame = true; } else { hoverLastFrame = false; }
        if (IsPressed() && Input.GetMouseButtonDown(0) && selectsoundKey > -1)
        {
            SelectSound();
        }
    }
    void HoverSound()
    {
        if (useCustomSoundKey)
        {
            if (isMainMenu) { mainMenu.usp.PlaySoundByKey(soundKey); }
            else { Debug.LogError("NO WHERE TO PLAY SOUND"); } //IN LEVEL SOUND OVERHOVER
        }
        else
        {
            if (isMainMenu) { mainMenu.usp.UISelectSound(0); }
            else { Debug.LogError("NO WHERE TO PLAY SOUND"); } //IN LEVEL SOUND OVERHOVER
        }
    }
    void SelectSound()
    {
        if (useCustomSelectSoundKey)
        {
            if (isMainMenu) { mainMenu.usp.PlaySoundByKey(selectsoundKey); }
            else { Debug.LogError("NO WHERE TO PLAY SOUND"); } //IN LEVEL SOUND OVERHOVER
        }
        else
        {
            if (isMainMenu) { mainMenu.usp.UISelectSound(0); }
            else { Debug.LogError("NO WHERE TO PLAY SOUND"); } //IN LEVEL SOUND OVERHOVER
        }
    }
}
