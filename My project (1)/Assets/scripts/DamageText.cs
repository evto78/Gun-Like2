using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DamageText : MonoBehaviour
{

    public Color normalHit;
    public Color critHit;
    public Color weakHit;
    public Color critWeakHit;
    public Color badHit;

    public TextMeshPro textDisplay;

    float timer;

    private void Start()
    {
        timer = 3f;
        Debug.Log("hello");
        transform.position = Vector3.zero;
    }

    public void SetText(string sentText, string givenColor)
    {
        textDisplay.text = sentText;
        if (givenColor == "normalHit")
        {
            textDisplay.color = normalHit;
        }
        if (givenColor == "critHit")
        {
            textDisplay.color = critHit;
        }
        if (givenColor == "weakHit")
        {
            textDisplay.color = weakHit;
        }
        if (givenColor == "critWeakHit")
        {
            textDisplay.color = critWeakHit;
        }
        if (givenColor == "badHit")
        {
            textDisplay.color = badHit;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y+(timer/40f), transform.position.z);

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
