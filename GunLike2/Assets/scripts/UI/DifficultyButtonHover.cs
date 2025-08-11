using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtonHover : Selectable
{
    MainMenuManager manager;
    public int difficultyRepresented; bool hoveredLastFrame = false;
    void Update()
    {
        if (manager == null) { manager = GameObject.Find("Main Menu Manager").GetComponent<MainMenuManager>(); }
        if (IsHighlighted()) { if (hoveredLastFrame) { return; } else { manager.OnHoverOver(true); manager.UpdateDifficultyDisplayedInfo(difficultyRepresented); } hoveredLastFrame = true; } else { hoveredLastFrame = false; }
    }
}
