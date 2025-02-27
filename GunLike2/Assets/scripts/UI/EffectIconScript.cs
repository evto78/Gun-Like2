using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectIconScript : MonoBehaviour
{
    public List<Sprite> effectSprites;
    public TextMeshProUGUI stacksTxt;

    public void UpdateEffectIcon(int id, int stacks)
    {
        stacksTxt.text = stacks.ToString();
        gameObject.GetComponent<Image>().sprite = effectSprites[id];
    }
}
