using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    public List<MeshRenderer> outlineTypes; float dangerTimer = 0f; bool updated = false;
    [System.Serializable]
    public enum State { off, disable, interactable, noMoney, danger}
    State curState;
    public bool disabledObject;
    public bool dangerousObject;
    private void Update()
    {
        if (curState == State.danger && updated)
        {
            ClearOutlines();
            if (dangerTimer > 0) { outlineTypes[3].enabled = true; }
            else { outlineTypes[4].enabled = true; }
            dangerTimer += Time.deltaTime * 10f; if (dangerTimer > 1) { dangerTimer = -1f; }
        }
    }
    private void LateUpdate()
    {
        if (!updated) { ChangeState(State.off); }
        updated = false;
    }
    public void ChangeState(State newState)
    {
        updated = true;
        if (curState != newState) { ClearOutlines(); }
        curState = newState;
        switch (curState)
        {
            case State.disable:
                outlineTypes[0].enabled = true;
                break;
            case State.interactable:
                outlineTypes[1].enabled = true;
                break;
            case State.noMoney:
                outlineTypes[2].enabled = true;
                break;
        }
    }
    void ClearOutlines()
    {
        outlineTypes[0].enabled = false;
        outlineTypes[1].enabled = false;
        outlineTypes[2].enabled = false;
        outlineTypes[3].enabled = false;
        outlineTypes[4].enabled = false;
    }
}
