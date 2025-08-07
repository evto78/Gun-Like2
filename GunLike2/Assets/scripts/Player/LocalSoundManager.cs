using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSoundManager : MonoBehaviour
{
    public float masterVol;
    public float musicVol;
    public float effectVol;
    public float uiVol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateSettings()
    {
        masterVol = PlayerPrefs.GetFloat("MASTERVOL");
        musicVol = PlayerPrefs.GetFloat("MUSICVOL");
        effectVol = PlayerPrefs.GetFloat("EFFECTVOL");
        uiVol = PlayerPrefs.GetFloat("UIVOL");
    }
}
