using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyBindButton : MonoBehaviour
{
    MainMenuManager mmm; GameDataManager gdm; SaveFileReadWrite instance; 
    public enum Bind { WF,WB,WL,WR,JUMP,SLAM,SLIDE,SPRINT,LI,RI,SHOW,LS,LR,RS,RR,PAUSE,INV}
    public Bind keyBindID; bool waiting;
    KeyCode oldBind;
    public TextMeshProUGUI keybindDisplay;

    void Start()
    {
        waiting = false;
        FindReferences();
        oldBind = GetBind();
        keybindDisplay.text = oldBind.ToString();
    }
    void FindReferences()
    {
        if (GameObject.Find("Main Menu Manager") != null) { mmm = GameObject.Find("Main Menu Manager").GetComponent<MainMenuManager>(); instance = mmm.instance; }
        if (GameObject.FindGameObjectWithTag("gdm") != null) { gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>(); instance = gdm.instance; }
    }
    public void NewBind()
    {
        if (waiting) { return; } waiting = true;

        oldBind = GetBind();

        StartCoroutine(WaitForNewBindInput());
    }
    KeyCode GetBind()
    {
        if (instance == null) { FindReferences(); }
        switch (keyBindID)
        {
            case Bind.WF: return instance.controlsBinds.walkForward;
            case Bind.WB: return instance.controlsBinds.walkBackward;
            case Bind.WL: return instance.controlsBinds.walkLeft;
            case Bind.WR: return instance.controlsBinds.walkRight;
            case Bind.JUMP: return instance.controlsBinds.jump;
            case Bind.SLAM: return instance.controlsBinds.slam;
            case Bind.SLIDE: return instance.controlsBinds.slide;
            case Bind.SPRINT: return instance.controlsBinds.sprint;
            case Bind.LI: return instance.controlsBinds.leftInteract;
            case Bind.RI: return instance.controlsBinds.righInteract;
            case Bind.SHOW: return instance.controlsBinds.showMoreInformation;
            case Bind.LS: return instance.controlsBinds.leftShoot;
            case Bind.LR: return instance.controlsBinds.leftReload;
            case Bind.RS: return instance.controlsBinds.rightShoot;
            case Bind.RR: return instance.controlsBinds.rightReload;
            case Bind.PAUSE: return instance.controlsBinds.pauseMenu;
            case Bind.INV: return instance.controlsBinds.openInventory;
            default: return KeyCode.Alpha0; // <- should never reach
        }
    }
    IEnumerator WaitForNewBindInput()
    {
        keybindDisplay.text = ">PRESS A KEY<";
        while (!Input.anyKey) { yield return new WaitForEndOfFrame(); }
        foreach (KeyCode kcode in KeyCode.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode) || Input.GetKeyDown(kcode)) 
            {
                switch (keyBindID)
                {
                    case Bind.WF: instance.controlsBinds.walkForward = kcode; break;
                    case Bind.WB: instance.controlsBinds.walkBackward = kcode; break;
                    case Bind.WL: instance.controlsBinds.walkLeft = kcode; break;
                    case Bind.WR: instance.controlsBinds.walkRight = kcode; break;
                    case Bind.JUMP: instance.controlsBinds.jump = kcode; break;
                    case Bind.SLAM: instance.controlsBinds.slam = kcode; break;
                    case Bind.SLIDE: instance.controlsBinds.slide = kcode; break;
                    case Bind.SPRINT: instance.controlsBinds.sprint = kcode; break;
                    case Bind.LI: instance.controlsBinds.leftInteract = kcode; break;
                    case Bind.RI: instance.controlsBinds.righInteract = kcode; break;
                    case Bind.SHOW: instance.controlsBinds.showMoreInformation = kcode; break;
                    case Bind.LS: instance.controlsBinds.leftShoot = kcode; break;
                    case Bind.LR: instance.controlsBinds.leftReload = kcode; break;
                    case Bind.RS: instance.controlsBinds.rightShoot = kcode; break;
                    case Bind.RR: instance.controlsBinds.rightReload = kcode; break;
                    case Bind.PAUSE: instance.controlsBinds.pauseMenu = kcode; break;
                    case Bind.INV: instance.controlsBinds.openInventory = kcode; break;
                }
            }
        }
        keybindDisplay.text = GetBind().ToString();
        instance.UpdateControls();
        waiting = false;
        yield return null;
    }
}
