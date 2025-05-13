using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DONTDESTROYONLOAD : MonoBehaviour
{
    private static DONTDESTROYONLOAD instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
