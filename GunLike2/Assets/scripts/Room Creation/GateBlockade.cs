using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBlockade : MonoBehaviour
{
    public Animator anim;
    public void Toggle(bool closed)
    {
        anim.SetBool("Closed", closed);
    }
}
