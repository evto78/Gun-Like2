using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public RemoteDoor bgDoor;
    Camera cam;
    bool starting;
    public List<GameObject> uiTOHIDE; bool loading;
    private void Awake()
    {
        Time.timeScale = 1f;
    }
    void Start()
    {
        Time.timeScale = 1f;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (starting && cam.transform.localScale.x == 2)
        {
            //
        }
    }

    public void Play(string what)
    {
        if(what == "new")
        {
            if (!starting)
            {
                bgDoor.Activate();
                cam.GetComponent<Animator>().SetBool("Leaving", true);
                starting = true;
                foreach (GameObject go in uiTOHIDE)
                {
                    go.SetActive(false);
                }
            }
        }
    }
    public void ExitGameButton()
    {
        Application.Quit();
    }
    public void LoadLevel()
    {
        if (loading) { return; } loading = true;
        StartCoroutine(LoadYourAsyncScene("Level Generation"));
    }
    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
