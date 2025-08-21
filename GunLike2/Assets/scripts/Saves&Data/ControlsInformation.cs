using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlsInformation
{
    public KeyCode walkForward;
    public KeyCode walkBackward;
    public KeyCode walkLeft;
    public KeyCode walkRight;

    public KeyCode jump;
    public KeyCode slam;
    public KeyCode slide;
    public KeyCode sprint;

    public KeyCode leftInteract;
    public KeyCode righInteract;
    public KeyCode showMoreInformation;

    public KeyCode leftShoot;
    public KeyCode leftReload;

    public KeyCode rightShoot;
    public KeyCode rightReload;

    public KeyCode pauseMenu;
    public KeyCode openInventory;

    public void DefaultControls()
    {
        walkForward = KeyCode.None;
        walkForward = KeyCode.W;
        walkBackward = KeyCode.S;
        walkLeft = KeyCode.A;
        walkRight = KeyCode.D;

        jump = KeyCode.Space;
        slam = KeyCode.LeftControl;
        slide = KeyCode.LeftControl;
        sprint = KeyCode.LeftShift;

        leftInteract = KeyCode.Q;
        righInteract = KeyCode.E;
        showMoreInformation = KeyCode.C;

        leftShoot = KeyCode.Mouse0;
        leftReload = KeyCode.R;

        rightShoot = KeyCode.Mouse1;
        rightReload = KeyCode.R;

        pauseMenu = KeyCode.Escape;
        openInventory = KeyCode.Tab;
    }
}
