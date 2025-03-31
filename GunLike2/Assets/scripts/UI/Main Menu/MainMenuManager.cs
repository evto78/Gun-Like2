using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public RemoteDoor bgDoor;
    Camera cam;
    bool starting;
    public List<GameObject> uiTOHIDE;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (starting && cam.transform.localScale.x == 2)
        {
            SceneManager.LoadScene("Sandbox");
        }
    }

    public void Play()
    {
        if (!starting)
        {
            bgDoor.Activate();
            cam.GetComponent<Animator>().SetBool("Leaving", true);
            starting = true;
            foreach(GameObject go in uiTOHIDE)
            {
                go.SetActive(false);
            }
        }
    }
}
