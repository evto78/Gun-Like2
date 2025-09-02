using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyRotate : MonoBehaviour
{
    public float spinSpeed; float spinProgress = 0;
    void Update()
    {
        spinProgress += Time.deltaTime * spinSpeed; if (spinProgress > 360) { spinProgress = spinProgress - 360; }
        transform.localEulerAngles = new Vector3(0, -50 + spinProgress, 0);
    }
}
