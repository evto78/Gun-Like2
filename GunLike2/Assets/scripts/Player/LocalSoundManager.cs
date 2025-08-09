using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSoundManager : MonoBehaviour
{
    public float masterVol;
    public float musicVol;
    public float effectVol;
    public float uiVol;
    public int channels;
    public Transform sourceHolder;
    List<AudioSource> lowPriorSources; int lastLowSource = 0;
    List<AudioSource> medPriorSources; int lastMedSource = 0;
    List<AudioSource> highPriorSources; int lastHighSource = 0;
    List<AudioSource> ultPriorSources; int lastUltSource = 0;
    public enum SoundType { music, effect, ui, other }
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
    public void PlayLocalSound(AudioClip incomingClip, string givinType, int prior)
    {
        SoundType type = SoundType.music;
        switch (givinType)
        {
            case "music": type = SoundType.music; break;
            case "effect": type = SoundType.effect; break;
            case "ui": type = SoundType.ui; break;
            case "other": type = SoundType.other; break;
        }
        AudioSource source = PickSource(prior);
        switch (type)
        {
            case SoundType.music: source.volume = (musicVol / 100f) * (masterVol / 100f); break;
            case SoundType.effect: source.volume = (effectVol / 100f) * (masterVol / 100f); break;
            case SoundType.ui: source.volume = (uiVol / 100f) * (masterVol / 100f); break;
            case SoundType.other: source.volume = (masterVol / 100f); break;
        }
        source.clip = incomingClip;
        source.Play();
    }
    AudioSource PickSource(int prior)
    {
        switch (prior)
        {
            case 0: lastLowSource++; if (lastLowSource >= lowPriorSources.Count) { lastLowSource = 0; } return lowPriorSources[lastLowSource];
            case 1: lastMedSource++; if (lastMedSource >= medPriorSources.Count) { lastMedSource = 0; } return medPriorSources[lastMedSource];
            case 2: lastHighSource++; if (lastHighSource >= highPriorSources.Count) { lastHighSource = 0; } return highPriorSources[lastHighSource];
            case 3: lastUltSource++; if (lastUltSource >= ultPriorSources.Count) { lastUltSource = 0; } return ultPriorSources[lastUltSource];
        }
        return lowPriorSources[0];
    }
}
