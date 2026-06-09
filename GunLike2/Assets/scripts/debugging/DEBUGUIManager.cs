using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DEBUGUIManager : MonoBehaviour
{
    UIManager uiMan;
    Rigidbody rb;
    GameDataManager gdm;

    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI velText;
    public TextMeshProUGUI activePointsText;
    public TextMeshProUGUI pointsLeftText;
    public TextMeshProUGUI pointsOnRestoreText;
    public TextMeshProUGUI pointRegenTimerText;

    void Start()
    {
        uiMan = GetComponentInParent<UIManager>();
        rb = GetComponentInParent<Rigidbody>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) { transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf); }

        fpsText.text = "FPS: " + (int)(1f / Time.unscaledDeltaTime);
        velText.text = "VEL: " + Mathf.Round(rb.velocity.magnitude).ToString();
        activePointsText.text = "Points Active: " + (int)gdm.activePoints;
        pointsLeftText.text = "Points Left: " + (int)gdm.pointsLeft;

        float pointsGiven = (gdm.basePoints.x + gdm.basePoints.y) / 2f;
        pointsGiven += gdm.flatPointsPerDifficulty * gdm.difficulty;
        pointsGiven *= gdm.difficulty / 2f;
        pointsGiven *= 1 + (0.5f * (gdm.phm.playerItem.leftItems[185] + gdm.phm.playerItem.rightItems[185]));

        pointsOnRestoreText.text = "Points On Restore: " + (int)pointsGiven;
        pointRegenTimerText.text = "Point Regen Timer: " + (60 - Mathf.FloorToInt(gdm.pointregenTimer));
    }
}
