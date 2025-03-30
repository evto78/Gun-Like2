using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public RemoteDoor bgDoor;
    Camera cam;
    bool starting;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            bgDoor.Activate();
            cam.GetComponent<Animator>().SetBool("Leaving", true);
            starting = true;
        }
        if (starting && cam.transform.localScale.x == 2)
        {
            SceneManager.LoadScene("Sandbox");
        }
    }
}
