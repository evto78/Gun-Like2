using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Animator anim; public GameObject assignedTar; bool hit; public bool assigned;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if(Physics.Raycast(new Ray(transform.position, -Vector3.up),out RaycastHit hit))
        {
            transform.position = hit.point;
        }
    }
    private void Update()
    {
        if(assignedTar == null && !hit && assigned) { Hit(); hit = true; }
    }
    public void Hit()
    {
        if(anim != null) { anim.SetTrigger("Hit"); }
       
        Destroy(gameObject, 0.05f);
    }
}
