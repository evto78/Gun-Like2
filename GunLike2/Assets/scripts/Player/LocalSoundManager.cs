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
    List<AudioSource> noPriorSources; int lastNoSource = 0;
    List<AudioSource> lowPriorSources; int lastLowSource = 0;
    List<AudioSource> medPriorSources; int lastMedSource = 0;
    List<AudioSource> highPriorSources; int lastHighSource = 0;
    List<AudioSource> ultPriorSources; int lastUltSource = 0;
    List<AudioClip> noHeldClip;
    List<AudioClip> lowHeldClip;
    List<AudioClip> medHeldClip;
    List<AudioClip> highHeldClip;
    List<AudioClip> ultHeldClip;
    public enum SoundType { music, effect, ui, other }
    // Start is called before the first frame update
    void Start()
    {
        noPriorSources = new List<AudioSource>();
        lowPriorSources = new List<AudioSource>();
        medPriorSources = new List<AudioSource>();
        highPriorSources = new List<AudioSource>();
        ultPriorSources = new List<AudioSource>();
        noHeldClip = new List<AudioClip>();
        lowHeldClip = new List<AudioClip>();
        medHeldClip = new List<AudioClip>();
        highHeldClip = new List<AudioClip>();
        ultHeldClip = new List<AudioClip>();
        for (int i = 0; i < channels; i++)
        {
            noPriorSources.Add(sourceHolder.gameObject.AddComponent<AudioSource>()); noHeldClip.Add(null);
            lowPriorSources.Add(sourceHolder.gameObject.AddComponent<AudioSource>()); lowHeldClip.Add(null);
            medPriorSources.Add(sourceHolder.gameObject.AddComponent<AudioSource>()); medHeldClip.Add(null);
            highPriorSources.Add(sourceHolder.gameObject.AddComponent<AudioSource>()); highHeldClip.Add(null);
            ultPriorSources.Add(sourceHolder.gameObject.AddComponent<AudioSource>()); ultHeldClip.Add(null);
        }
        for (int i = 0; i < channels; i++)
        {
            AudioSourceSetUp(255, noPriorSources[i]);
            AudioSourceSetUp(150, lowPriorSources[i]);
            AudioSourceSetUp(50, medPriorSources[i]);
            AudioSourceSetUp(25, highPriorSources[i]);
            AudioSourceSetUp(0, ultPriorSources[i]);
        }
    }
    void AudioSourceSetUp(int priority, AudioSource source)
    {
        source.priority = priority;
        source.dopplerLevel = 0;
        source.minDistance = 400;
        source.playOnAwake = false;
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
        AudioSource existingSource = null;
        switch (prior)
        {
            case -1: if (noHeldClip.Contains(incomingClip)) { existingSource = noPriorSources[noHeldClip.IndexOf(incomingClip)]; } break;
            case 0: if (lowHeldClip.Contains(incomingClip)) { existingSource = lowPriorSources[lowHeldClip.IndexOf(incomingClip)]; } break;
            case 1: if (medHeldClip.Contains(incomingClip)) { existingSource = medPriorSources[medHeldClip.IndexOf(incomingClip)]; } break;
            case 2: if (highHeldClip.Contains(incomingClip)) { existingSource = highPriorSources[highHeldClip.IndexOf(incomingClip)]; } break;
            case 3: if (ultHeldClip.Contains(incomingClip)) { existingSource = ultPriorSources[ultHeldClip.IndexOf(incomingClip)]; } break;
        }
        if (existingSource != null)
        {
            SoundType type = SoundType.music;
            switch (givinType)
            {
                case "music": type = SoundType.music; break;
                case "effect": type = SoundType.effect; break;
                case "ui": type = SoundType.ui; break;
                case "other": type = SoundType.other; break;
            }
            switch (type)
            {
                case SoundType.music: existingSource.volume = (musicVol / 100f) * (masterVol / 100f); break;
                case SoundType.effect: existingSource.volume = (effectVol / 100f) * (masterVol / 100f); break;
                case SoundType.ui: existingSource.volume = (uiVol / 100f) * (masterVol / 100f); break;
                case SoundType.other: existingSource.volume = (masterVol / 100f); break;
            }
            if (existingSource.volume == 0) { return; }
            existingSource.Play();
        }
        else
        {
            SoundType type = SoundType.music;
            switch (givinType)
            {
                case "music": type = SoundType.music; break;
                case "effect": type = SoundType.effect; break;
                case "ui": type = SoundType.ui; break;
                case "other": type = SoundType.other; break;
            }
            AudioSource source = PickSource(prior, incomingClip);
            if (source.isPlaying) { source = PickSource(prior + 1, incomingClip); } //switch to a higher channel if the previous one is busy
            switch (type)
            {
                case SoundType.music: source.volume = (musicVol / 100f) * (masterVol / 100f); break;
                case SoundType.effect: source.volume = (effectVol / 100f) * (masterVol / 100f); break;
                case SoundType.ui: source.volume = (uiVol / 100f) * (masterVol / 100f); break;
                case SoundType.other: source.volume = (masterVol / 100f); break;
            }
            if (source.volume == 0) { return; }
            source.clip = incomingClip;
            source.Play();
        }
    }
    AudioSource PickSource(int prior, AudioClip clip)
    {
        switch (prior)
        {
            case -1: lastNoSource++; if (lastNoSource >= noPriorSources.Count) { lastNoSource = 0; } noHeldClip[lastNoSource] = clip; return noPriorSources[lastNoSource];
            case 0: lastLowSource++; if (lastLowSource >= lowPriorSources.Count) { lastLowSource = 0; } lowHeldClip[lastLowSource] = clip; return lowPriorSources[lastLowSource];
            case 1: lastMedSource++; if (lastMedSource >= medPriorSources.Count) { lastMedSource = 0; } medHeldClip[lastMedSource] = clip; return medPriorSources[lastMedSource];
            case 2: lastHighSource++; if (lastHighSource >= highPriorSources.Count) { lastHighSource = 0; } highHeldClip[lastHighSource] = clip; return highPriorSources[lastHighSource];
            case 3: lastUltSource++; if (lastUltSource >= ultPriorSources.Count) { lastUltSource = 0; } ultHeldClip[lastUltSource] = clip; return ultPriorSources[lastUltSource];
            case 4: return (PickSource(0, clip));
        }
        return PickSource(0, clip);
    }
}
