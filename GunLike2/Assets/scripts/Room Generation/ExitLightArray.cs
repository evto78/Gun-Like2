using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLightArray : MonoBehaviour
{
    List<Light> lights = new List<Light>();
    public AnimationCurve curve;
    public float maxIntensity;
    public float minIntensity;
    public float speed;
    float numOfLights;
    float timer;
    private void Start()
    {
        lights.AddRange(GetComponentsInChildren<Light>());
        foreach(Light light in lights) { light.intensity = minIntensity;}
        timer = 0f;
        numOfLights = lights.Count;
        
    }
    private void Update()
    {
        timer += Time.deltaTime * speed;

        for(float i = 1; i < lights.Count+1; i++)
        {
            lights[Mathf.RoundToInt(i-1)].intensity = Mathf.Clamp(curve.Evaluate(((Mathf.Clamp(timer, (i-1f)/numOfLights, (i)/numOfLights))*6f)/i) * maxIntensity, minIntensity, maxIntensity);
        }

        if(timer >= 1) { timer = 0f; }
    }
}
