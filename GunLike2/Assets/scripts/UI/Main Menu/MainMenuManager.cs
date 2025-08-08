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
    public SaveFileReadWrite instance;
    WeaponSelection weaponSelect;
    private void Awake()
    {
        weaponSelect = GameObject.Find("BEHINDDOORUI").GetComponent<WeaponSelection>();
        Time.timeScale = 1f;
    }
    void Start()
    {
        Time.timeScale = 1f;
        cam = Camera.main;
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
    void UpdateGunInfo()
    {
        instance.data.gunInfo[PlayerPrefs.GetInt("leftHandGunSelect")].runs++;
        instance.data.gunInfo[PlayerPrefs.GetInt("rightHandGunSelect")].runs++;
        instance.UpdateSaveFile();
    }
    public void ExitGameButton()
    {
        Application.Quit();
    }
    public void LoadLevel()
    {
        if (loading) { return; } loading = true;
        UpdateGunInfo();
        StartCoroutine(LoadAsyncScene("Level Generation"));
    }
    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
