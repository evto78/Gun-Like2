using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connectiveRoomScript : MonoBehaviour
{
    public GameObject frontIndicator;
    public GameObject backIndicator;

    public Material mainSign;
    public Material sideSign;

    bool mainPath;

    public void IsMain(bool main)
    {
        mainPath = main;
    }

    private void Update()
    {
        if (mainPath)
        {
            frontIndicator.GetComponent<MeshRenderer>().material = mainSign;
            backIndicator.GetComponent<MeshRenderer>().material = mainSign;
        }
        else
        {
            frontIndicator.GetComponent<MeshRenderer>().material = sideSign;
            backIndicator.GetComponent<MeshRenderer>().material = mainSign;
        }
    }
}
