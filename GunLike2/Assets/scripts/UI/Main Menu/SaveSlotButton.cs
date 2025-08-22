using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SaveSlotButton : MonoBehaviour
{
    public TextMeshProUGUI runName;
    public TextMeshProUGUI runDescription;
    Button btn;
    private void Start()
    {
        btn = GetComponent<Button>();
    }
    public void SaveSlotSetUp(RunSaveData data)
    {
        runName.text = "Empty"; runDescription.text = "";
        if (btn == null) { btn = GetComponent<Button>(); }
        if (data == null) { btn.interactable = false; return; }

        runName.text = data.runName;
        switch (data.selectedDifficulty)
        {
            case 0: runDescription.text = "Relaxed, Room " + data.roomNumber + ", Items: " + (data.leftInv.Sum()+data.rightInv.Sum()); break;
            case 1: runDescription.text = "Standard, Room " + data.roomNumber + ", Items: " + (data.leftInv.Sum() + data.rightInv.Sum()); break;
            case 2: runDescription.text = "Irradiated, Room " + data.roomNumber + ", Items: " + (data.leftInv.Sum() + data.rightInv.Sum()); break;
            case 3: runDescription.text = "Nuclear, Room " + data.roomNumber + ", Items: " + (data.leftInv.Sum() + data.rightInv.Sum()); break;
            case 4: runDescription.text = "Mutated, Room " + data.roomNumber + ", Items: " + (data.leftInv.Sum() + data.rightInv.Sum()); break;
        }
    }
}
