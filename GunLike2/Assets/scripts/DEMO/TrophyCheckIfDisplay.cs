using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyCheckIfDisplay : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.HasKey("Victory") && PlayerPrefs.GetInt("Victory") == 1) { }
        else { gameObject.SetActive(false); }
    }
}
