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

    public Vector3 drift;
    public float driftStr;

    public TextMeshProUGUI textDisplay;

    public Camera myCamera;

    float timer;

    private void Start()
    {
        timer = 2f;
    }

    public void SetText(string sentText, string givenColor, Vector3 worldPos, string source)
    {
        myCamera = Camera.main;

        float tempRounding;
        float.TryParse(sentText, out tempRounding);
        tempRounding = Mathf.CeilToInt(tempRounding);
        textDisplay.text = ""+tempRounding;
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

        driftStr = Random.Range(10f, 15f);

        if (source == "left")
        {
            drift = new Vector3(Random.Range(-0.7f, -0.3f), Random.Range(0.5f, 1f), 1f);
        }
        if (source == "right")
        {
            drift = new Vector3(Random.Range(0.7f, 0.3f), Random.Range(0.5f, 1f), 1f);
        }
        if (source == "self")
        {
            drift = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0.5f, 1f), 1f);
        }
        
    }

    void Update()
    {
        if(myCamera == null) { myCamera = Camera.main; }

        //transform.position = myRelatieWorldPos;

        //transform.LookAt(myCamera.transform);
        transform.localEulerAngles = new Vector3(0, 0, 0);

        timer -= Time.deltaTime;

        //transform.position = myCamera.WorldToScreenPoint(myRelatieWorldPos);
        //transform.position = new Vector3(transform.position.x, transform.position.y, 1f);

        transform.position = PositionWithDrift();

        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        ManageFadeOut();
    }

    private void FixedUpdate()
    {
        transform.position = myCamera.WorldToScreenPoint(myRelatieWorldPos);
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
    }

    void ManageFadeOut()
    {
        textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, 100f * (timer / 2f) );
    }

    Vector3 PositionWithDrift()
    {
        Vector3 newPos;

        transform.position = myCamera.WorldToScreenPoint(myRelatieWorldPos);
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);

        newPos = transform.position + drift * (driftStr * (2f - timer));

        return newPos;
    }
}
