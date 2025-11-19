using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectIconScript : MonoBehaviour
{
    public TextMeshProUGUI stacksTxt;
    public void UpdateEffectIcon(Sprite spriteToUse, int stacks)
    {
        stacksTxt.text = stacks.ToString();
        gameObject.GetComponent<Image>().sprite = spriteToUse;
    }
}
