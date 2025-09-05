using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeadlineScript : MonoBehaviour
{
    GameDataManager gdm;

    public TextMeshProUGUI timerTxt;
    float timeLeft; bool timerActive;
    float internalDeadlineTimer;
    float speedMod;

    float blinkTimer; bool blinked;

    public List<Transform> gears;
    List<Image> imgs;

    void Start()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        internalDeadlineTimer = 0f; blinkTimer = 0f; blinked = false;
        SetTimer(480f, false, 1f); //Set timer to 8 mins
    }
    void Update()
    {
        timerTxt.color = new Color(1f, 1f, 1f, 1f);
        if (timerActive) 
        { 
            timerTxt.text = SecondsToMinSec(timeLeft);

            for (int i = 0; i < gears.Count; i++) {
                if(i == 1) { gears[i].Rotate(0, 0, -Time.deltaTime * speedMod * 10f); }
                else { gears[i].Rotate(0, 0, Time.deltaTime * speedMod * 10f); } }

            timeLeft -= Time.deltaTime * speedMod;
            if(timeLeft <= 0)
            {
                timeLeft = 0;
                internalDeadlineTimer -= Time.deltaTime;
                Blink();
                if( internalDeadlineTimer < 0)
                {
                    internalDeadlineTimer = 1f;
                    if (Random.Range(1f, 100f) <= 10f) { gdm.DeadLine(); }
                }
            }
        }
        else
        {
            timerTxt.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }
    void Blink()
    {
        blinkTimer -= Time.deltaTime;
        if(blinkTimer < 0)
        {
            blinkTimer = 0.5f;
            blinked = !blinked;
        }
        if (blinked)
        {
            timerTxt.color = new Color(1f, 1f, 1f, 0.3f);
        }
        else
        {
            timerTxt.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    string SecondsToMinSec(float input)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);
        return timeFormatted;
    }
    public void SetTimer(float timeLeftGiven, bool active, float speed)
    {
        speedMod = speed;
        timeLeft = timeLeftGiven; timerActive = active;
        timerTxt.text = SecondsToMinSec(timeLeft);
    }
}
