using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightingManagerScript : MonoBehaviour
{
    public LightingSettings defaultLighting;
    public List<LightingSettings> altLighting;
    void Start() { Lightmapping.lightingSettings = defaultLighting; }
    public void ChangeLighting(int id) { Lightmapping.lightingSettings = altLighting[id]; }
}
