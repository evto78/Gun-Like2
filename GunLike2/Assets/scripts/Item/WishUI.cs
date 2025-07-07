using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishUI : MonoBehaviour
{
    float width;
    RectTransform rTran;
    public AnimationCurve popOutSmoothing;
    float timerOut; public bool ready;
    void Start()
    {
        timerOut = 1f;
        rTran = GetComponent<RectTransform>();
        width = rTran.rect.width;
        Popout();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerOut < 1) { timerOut += Time.deltaTime; rTran.position = new Vector2((width * popOutSmoothing.Evaluate(timerOut)) + Screen.width, rTran.position.y); }
    }
    public int readWish()
    {
        if (ready)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)){ return 1; }//Heal
            else if (Input.GetKeyDown(KeyCode.Alpha2)){ return 2; }//Money
            else if (Input.GetKeyDown(KeyCode.Alpha3)){ return 3; }//Item
            else if (Input.GetKeyDown(KeyCode.Alpha4)){ return 4; }//Smite
            else { return 0; }
        }
        else { return 0; }
    }
    public void Popout()
    {
        timerOut = 0f;
    }
}
