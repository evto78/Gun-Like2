using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostDisplay : MonoBehaviour
{
    public float offset;
    public bool generateOwnPrice;
    TextMeshProUGUI textmesh;
    private void Start()
    {
        GameDataManager gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        textmesh = GetComponentInChildren<TextMeshProUGUI>();
        if (generateOwnPrice) { textmesh.text = (Mathf.CeilToInt((gdm.phm.baseCost * (int)(gdm.difficulty * (gdm.roomNumber + 1))) * 0.5f)).ToString()+"$"; }
    }
    void Update()
    {
        transform.position = transform.parent.position + Vector3.up * offset;
    }
}
