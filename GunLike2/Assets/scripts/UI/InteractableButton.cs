using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : MonoBehaviour
{

    public List<GameObject> linkedObjects;
    Animator animator;
    float interactTimer;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        interactTimer -= Time.deltaTime;
    }

    public void Interact()
    {
        if(interactTimer < 0f)
        {
            interactTimer = 1f;
            animator.SetTrigger("Press");
            if (linkedObjects.Count > 0)
            {
                foreach (GameObject thing in linkedObjects)
                {
                    thing.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
