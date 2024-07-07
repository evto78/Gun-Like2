using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
}

