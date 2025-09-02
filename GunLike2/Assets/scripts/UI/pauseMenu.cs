using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    SaveFileReadWrite instance;
    public void ExitGameButton()
    {
        Application.Quit();
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
