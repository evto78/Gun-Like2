using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteDoor : MonoBehaviour
{
    Animator animator;
    bool open;

    void Start()
    {
        animator = GetComponent<Animator>();
        open = false;
    }
    public void Activate()
    {
        if (open) { open = false; } else { open = true; }
        animator.speed = 1f;
        animator.SetBool("Open", open);
    }
    public void SetOpen(bool openSent)
    {
        open = openSent;
        animator.speed = 100f;
        animator.SetBool("Open", open);
    }
}
