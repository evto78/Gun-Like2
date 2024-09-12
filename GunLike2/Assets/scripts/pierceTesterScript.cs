using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class pierceTesterScript : MonoBehaviour
{
    public float curHp;
    public float maxHp;

    public float armor;

    public GameObject displaySheet;
    public Color myColor;

    float timer;

    void Start()
    {
        curHp = maxHp;
    }

    void Update()
    {
        displaySheet.GetComponent<Image>().color = myColor;

        if (timer < 0)
        {
            myColor = Color.red;
        }
        timer -= Time.deltaTime;
    }

    public void OnHit()
    {
        myColor = Color.green;
        timer = 10f;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}
