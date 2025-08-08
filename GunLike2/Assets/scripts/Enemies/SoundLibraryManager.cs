using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibraryManager : MonoBehaviour
{
    public enum SoundType { music,effect,ui,other}
    public List<AudioClip> monoSounds;
    public List<SoundType> soundTypes;
    public List<string> soundNames;
    public List<AudioSource> soundSources;
    LocalSoundManager soundManager;
    GameDataManager gdm;
    float masterVol;
    float musicVol;
    float effectVol;
    float uiVol;
    void Start()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        soundManager = gdm.phm.lsm;
        masterVol = soundManager.masterVol;
        musicVol = soundManager.musicVol;
        effectVol = soundManager.effectVol;
        uiVol = soundManager.uiVol;
    }
    public void PlaySoundByName(string key)
    {
        if (!soundNames.Contains(key)) { return; }
        int i = soundNames.IndexOf(key);
        if (soundSources[i] != null)
        {
            soundSources[i].clip = monoSounds[i];
            switch (soundTypes[i])
            {
                case SoundType.music: soundSources[i].volume = (musicVol/100f)*(masterVol/100f); break;
                case SoundType.effect: soundSources[i].volume = (effectVol / 100f) * (masterVol / 100f); break;
                case SoundType.ui: soundSources[i].volume = (uiVol / 100f) * (masterVol / 100f); break;
                case SoundType.other: soundSources[i].volume = (masterVol / 100f); break;
            }
            soundSources[i].Play();
        }
        else
        {
            soundManager.PlayLocalSound(monoSounds[i], LocalSoundManager.SoundType.music, 0); //FIX!!@!!
        }
    }
}
