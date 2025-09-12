using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecycleBinUI : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI costDisplay;
    public Animator animator;
    GameDataManager gdm;
    bool hovering;
    private void Start()
    {
        hovering = false;
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
    }
    public bool GetHighlighted()
    {
        return hovering;
    }
    private void OnEnable()
    {
        animator.SetTrigger("Hover");
    }
    private void Update()
    {
        hovering = Vector3.Distance(Input.mousePosition, transform.position) < 90;
        if (hovering)
        {
            img.color = Color.white;

            if(gdm.pi.itemHeld == -1) { return; }
            costDisplay.transform.parent.gameObject.SetActive(true);

            int cost = Mathf.CeilToInt((gdm.phm.baseCost * (int)(gdm.difficulty * (gdm.roomNumber + 1))) * 0.75f);

            switch (gdm.pi.FindRarityByID(gdm.pi.itemHeld))
            {
                case 0: cost *= 1; break;
                case 1: cost = Mathf.CeilToInt(cost * 1.5f); break;
                case 2: cost *= 2; break;
                case 3: cost *= 4; break;
                case 4: cost *= 3; break;
                case 5: cost *= 3; break;
                case 6: cost *= 3; break;
                case 7: cost *= 4; break;
                case 8: cost = 0; break;
            }

            costDisplay.text = cost + "$";
        }
        else
        {
            img.color = new Color(0.9f, 0.9f, 0.9f, 1);
            costDisplay.transform.parent.gameObject.SetActive(false);
        }
    }
}
