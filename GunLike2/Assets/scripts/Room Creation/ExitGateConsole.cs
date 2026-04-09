using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExitGateConsole : MonoBehaviour
{
    Collider collide;
    bool interacted;
    Animator anim;
    GateBlockade exitGate;
    GameDataManager gdm;
    UIManager ui;
    public TextMeshProUGUI consoleTxt;
    public TextMeshProUGUI line1Txt;
    public TextMeshProUGUI line2Txt;
    public TextMeshProUGUI line3Txt;
    public TextMeshProUGUI completionTxt;
    float textAnimTimer; int textAnimCycle; bool bossRoom; EnemyHealthManager bossHealth;
    // Start is called before the first frame update
    private void Awake()
    {
        collide = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        ui = gdm.phm.uiMan;
        exitGate = gdm.endGateBlockade;
    }
    void Start()
    {
        SetUp(false, null);
    }
    public void SetUp(bool isBoss, EnemyHealthManager bossEhm)
    {
        collide.enabled = true;
        bossRoom = isBoss; 
        anim.SetTrigger("Up");
        consoleTxt.text = "Awaiting Input...";
        line1Txt.text = "";
        line2Txt.text = "";
        line3Txt.text = "";
        completionTxt.text = "";
        textAnimTimer = 0f; textAnimCycle = 0;
        interacted = false;
        if(bossEhm == null) { bossHealth = null; } else { bossHealth = bossEhm; }
    }
    private void Update()
    {
        if (!interacted) 
        {
            textAnimTimer += Time.deltaTime; if(textAnimTimer > 0.5f) { textAnimCycle++; textAnimTimer = 0f; } if(textAnimCycle > 3) { textAnimCycle = 0; }
            switch (textAnimCycle)
            {
                case 0: consoleTxt.text = "Awaiting Input"; break;
                case 1: consoleTxt.text = "Awaiting Input."; break;
                case 2: consoleTxt.text = "Awaiting Input.."; break;
                case 3: consoleTxt.text = "Awaiting Input..."; break;
            }
        }
        else
        {
            ui.exitConsoleSideViewLines[0].text = consoleTxt.text;
            ui.exitConsoleSideViewLines[1].text = line1Txt.text;
            ui.exitConsoleSideViewLines[2].text = line2Txt.text;
            ui.exitConsoleSideViewLines[3].text = line3Txt.text;
            ui.exitConsoleSideViewLines[4].text = completionTxt.text;
        }
    }
    public void Interact()
    {
        if (interacted) { return; } interacted = true;
        StartCoroutine(UnlockSequence());
    }
    IEnumerator UnlockSequence()
    {
        ui.GateUnlockUpdate(false);
        gdm.PointsRestore();
        consoleTxt.text = "Input Recived!";
        yield return new WaitForSeconds(0.1f);
        consoleTxt.text = "Input Recived! Please Wait...";
        yield return new WaitForSeconds(0.1f);
        line1Txt.text = "Locking Openings... [0/100]"; float progression = 0;
        while (progression < 100)
        {
            progression += Time.deltaTime * 10f;
            line1Txt.text = "Locking Openings... [" + (int)progression + "/100]";
            yield return new WaitForEndOfFrame();
        }
        line1Txt.text = "Locking Openings... [LOCKED]";
        gdm.pointsLocked = true; ui.deadline.SetTimer(480f, false, 1f);
        yield return new WaitForSeconds(0.5f);
        line2Txt.text = "Unlocking Gate... [0/100]"; progression = 0f;
        while (progression < 100)
        {
            if(gdm.activeEhms.Count < 15 || (bossRoom && bossHealth == null)) 
            {
                progression += Time.deltaTime * 5f; line2Txt.color = new Color(1, 1, 1, 1); line3Txt.text = "";
            }
            else
            {
                progression -= Time.deltaTime / 2f; line2Txt.color = new Color(0.7f, 0.7f, 0.7f, 1f); line3Txt.text = "!!! ERROR !!!  MAJOR ACTIVITY DETECTED, CANNOT UNLOCK.";
                if(progression < 0) { progression = 0; }
            }
            
            line2Txt.text = "Unlocking Gate... [" + (int)progression + "/100]";
            yield return new WaitForEndOfFrame();
        }
        line3Txt.text = "";
        line2Txt.text = "Unlocking Gate... [UNLOCKED]";
        yield return new WaitForSeconds(1f);
        consoleTxt.text = ""; line1Txt.text = ""; line2Txt.text = ""; line3Txt.text = "";
        completionTxt.text = "Sequence Complete, Enjoy :)";
        yield return new WaitForSeconds(1f);
        anim.ResetTrigger("Up");
        anim.SetTrigger("Down");
        ui.GateUnlockUpdate(true);
        yield return new WaitForSeconds(0.5f);
        exitGate.Toggle(false);
        collide.enabled = false;
        gdm.PullItemsToPosition(transform.position - Vector3.forward * 10f);
        yield return null;
    }
}
