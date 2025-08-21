using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTip : MonoBehaviour
{
    public string hintID; public KeyCode dismissKey;
    private void Awake()
    {
        int display;
        if (!PlayerPrefs.HasKey("DISPLAYHINT" + hintID))
        {
            PlayerPrefs.SetInt("DISPLAYHINT" + hintID, 1);
        } 
        display = PlayerPrefs.GetInt("DISPLAYHINT" + hintID);
        if(display == 0) { gameObject.SetActive(false); }
    }
    void Update()
    {
        if (Input.GetKey(dismissKey)) { gameObject.SetActive(false); }
        if (Input.GetKey(dismissKey) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) { PlayerPrefs.SetInt("DISPLAYHINT" + hintID, 0); gameObject.SetActive(false); }
    }

}
