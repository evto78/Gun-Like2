using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibraryManager : MonoBehaviour
{
    public enum SoundType { music,effect,ui,other}
    public class Sounds
    {
        public AudioClip monoSound;
        public SoundType soundType;
        public string soundName;
        public AudioSource source;
        public int priority;
    }
    public List<Sounds> sounds;
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
    public void PlaySoundByName(int key)
    {
        int i = key;
        if (sounds[i].source != null)
        {
            sounds[i].source.clip = sounds[i].monoSound;
            switch (sounds[i].soundType)
            {
                case SoundType.music: sounds[i].source.volume = (musicVol/100f)*(masterVol/100f); break;
                case SoundType.effect: sounds[i].source.volume = (effectVol / 100f) * (masterVol / 100f); break;
                case SoundType.ui: sounds[i].source.volume = (uiVol / 100f) * (masterVol / 100f); break;
                case SoundType.other: sounds[i].source.volume = (masterVol / 100f); break;
            }
            sounds[i].source.Play();
        }
        else
        {
            soundManager.PlayLocalSound(sounds[i].monoSound, sounds[i].soundType.ToString(), 0);
        }
    }
}
