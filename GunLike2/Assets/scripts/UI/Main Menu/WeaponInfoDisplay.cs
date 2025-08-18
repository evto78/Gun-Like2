using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public GameObject textLine;
    public Transform effectListHolder;
    public void InfoUpdate(GunObjectData data)
    {
        int childCount = effectListHolder.childCount;
        foreach (Transform child in effectListHolder)
        {
            Destroy(child.gameObject);
        }
        title.text = data.gunName;
        description.text = data.descriptionText;
        foreach(string line in data.effectText)
        {
            Instantiate(textLine, effectListHolder).GetComponent<TextMeshProUGUI>().text = line;
        }
    }
}
