using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Popup : MonoBehaviour
{
    float startTimer;
    float timer;
    void Start()
    {
        startTimer = 1f;
        timer = 1f;
    }
    void Update()
    {
        startTimer -= Time.deltaTime;
        gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, timer / 2f);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<Image>(out Image image))
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, timer / 2f);
            }
            if (transform.GetChild(i).TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, timer);
            }
        }
        if (startTimer < 0.5f)
        {
            timer -= Time.deltaTime;
        }
    }
}
