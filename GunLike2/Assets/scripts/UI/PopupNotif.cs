using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNotif : MonoBehaviour
{
    RectTransform rectTransform;
    public bool state;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (state)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, new Vector3(0, rectTransform.position.y, rectTransform.position.z), Time.deltaTime*10);
        }
        else
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, new Vector3(-650, rectTransform.position.y, rectTransform.position.z), Time.deltaTime * 10);
        }
    }
}
