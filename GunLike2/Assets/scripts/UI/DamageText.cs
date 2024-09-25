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

    public Vector3 myRelatieWorldPos;

    public TextMeshPro textDisplay;

    public Camera myCamera;

    float timer;

    private void Start()
    {
        timer = 999f;
    }

    public void SetText(string sentText, string givenColor, Vector3 worldPos)
    {
        myCamera = Camera.main;

        textDisplay.text = sentText;
        myRelatieWorldPos = worldPos;
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
        if(myCamera == null) { myCamera = Camera.main; }

        timer -= Time.deltaTime;
        transform.position = myCamera.WorldToScreenPoint(myRelatieWorldPos);
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
