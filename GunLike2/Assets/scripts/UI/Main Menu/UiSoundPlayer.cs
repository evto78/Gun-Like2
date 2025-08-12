using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSoundPlayer : MonoBehaviour
{
    public LocalSoundManager lsm;
    [System.Serializable]
    public enum SoundType { music, effect, ui, other }
    [System.Serializable]
    public class Sounds
    {
        public string soundName;
        public AudioClip monoSound;
        public SoundType soundType;
        public int priority;
    }
    [System.Serializable]
    public class SimpleSounds
    {
        public string soundName;
        public AudioClip monoSound;
    }
    public List<Sounds> soundsOTHER; //no sepesific order
    public List<SimpleSounds> soundsUIGUNBUTTON; //0: lowest hover, 1: lowest select, 2: lowest difficulty select. 3: norm hover, 4: norm select, 5: norm difficulty select, etc
    public void PlaySoundByKey(int i)
    {
        lsm.PlayLocalSound(soundsOTHER[i].monoSound, soundsOTHER[i].soundType.ToString(), soundsOTHER[i].priority);
    }
    public void UIHoverSound(int difficulty)
    {
        lsm.PlayLocalSound(soundsUIGUNBUTTON[difficulty].monoSound, SoundType.ui.ToString(), 0);
    }
    public void UISelectSound(int difficulty)
    {
        lsm.PlayLocalSound(soundsUIGUNBUTTON[difficulty+1].monoSound, SoundType.ui.ToString(), 1);
    }
    public void UIDifficultySound(int difficulty)
    {
        lsm.PlayLocalSound(soundsUIGUNBUTTON[difficulty+2].monoSound, SoundType.ui.ToString(), 2);
    }
}
