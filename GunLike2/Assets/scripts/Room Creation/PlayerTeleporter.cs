using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    GameObject player; GameDataManager gdm;
    public GameObject start; public GameObject end;
    List<InteractableButton> startButtons = new List<InteractableButton>();
    List<RemoteDoor> startDoors = new List<RemoteDoor>();
    List<InteractableButton> endButtons = new List<InteractableButton>();
    List<RemoteDoor> endDoors = new List<RemoteDoor>();
    private void Start()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        player = gdm.phm.gameObject;
        startButtons.AddRange(start.GetComponentsInChildren<InteractableButton>());
        startDoors.AddRange(start.GetComponentsInChildren<RemoteDoor>());
        endButtons.AddRange(start.GetComponentsInChildren<InteractableButton>());
        endDoors.AddRange(start.GetComponentsInChildren<RemoteDoor>());
    }
    public void Activate()
    {
        foreach(InteractableButton button in startButtons) { button.oneTimePress = true; button.pressed = false; }
        foreach(InteractableButton button in endButtons) { button.oneTimePress = true; button.pressed = false; }
        foreach(RemoteDoor door in startDoors) { door.SetOpen(false); }
        foreach(RemoteDoor door in endDoors) { door.SetOpen(false); }
        player.transform.position = player.transform.position + (start.transform.position - end.transform.position);
        gdm.AdvanceToNextRoom();
    }
}
