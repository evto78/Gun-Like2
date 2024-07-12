using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DeveloperConsoleBehaviour : MonoBehaviour
{
    [SerializeField] private string prefix = string.Empty;
    [SerializeField] private ConsoleCommand[] commands = new ConsoleCommand[0];

    [Header("UI")]
    [SerializeField] private GameObject uiCanvas = null;
    [SerializeField] private TMP_InputField inputField = null;

    private float pausedTimeScale;
    private static DeveloperConsoleBehaviour instance;
    private DevConsole devConsole;
    private DevConsole developerConsole
    {
        get
        {
            if (devConsole != null) { return devConsole; }
            return devConsole = new DevConsole(prefix, commands);
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Toggle()
    {
        
        if (uiCanvas.activeSelf)
        {
            Time.timeScale = pausedTimeScale;
            uiCanvas.SetActive(false);
        }
        else
        {
            pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            uiCanvas.SetActive(true);
            inputField.ActivateInputField();
        }
    }
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Tilde))
        {
            Toggle();
            Debug.Log("toggled");
        }
    }
    public void ProcessCommand(string inputValue)
    {
        developerConsole.ProcessCommand(inputValue);

        inputField.text = string.Empty;
    }
}
