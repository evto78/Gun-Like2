using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorRoomOpenButton : MonoBehaviour
{
    public TextMeshProUGUI leftInteractBtn;
    public TextMeshProUGUI rightInteractBtn;
    GameDataManager gdm;
    void Start()
    {
        if (PlayerPrefs.HasKey("ShowInitialIndicatorFirstButton")) { if (PlayerPrefs.GetInt("ShowInitialIndicatorFirstButton") == 1) { Destroy(gameObject); } }
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        InvokeRepeating("UpdateShownInputs", 0.5f, 1f);
    }
    void UpdateShownInputs()
    {
        leftInteractBtn.text = gdm.instance.controlsBinds.leftInteract.ToString();
        rightInteractBtn.text = gdm.instance.controlsBinds.righInteract.ToString();
    }
    public void Activate()
    {
        PlayerPrefs.SetInt("ShowInitialIndicatorFirstButton", 1);
        Destroy(gameObject);
    }
}
