using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame : MonoBehaviour
{
    bool playerNear;
    bool playerFishing;
    GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void Update()
    {
        if (playerNear)
        {
            if(!playerFishing && Input.GetKeyDown(KeyCode.E))
            {
                playerFishing = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                player.GetComponent<UIManager>().fishing = true;
            }
        }
        if (playerFishing && Input.GetKeyDown(KeyCode.Escape))
        {
            playerFishing = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<UIManager>().fishing = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNear = false;
        }
    }
}
