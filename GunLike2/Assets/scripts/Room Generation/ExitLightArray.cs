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
            float min = (i - 1f) / numOfLights;
            float max = (i + 1f) / numOfLights;
            float relativeTimer = Mathf.Clamp(timer, min, max);
            relativeTimer = ((((relativeTimer/numOfLights)/max)*numOfLights*((min)-(relativeTimer))) * -1f)*10f;
            float curvePos = curve.Evaluate(relativeTimer);

            lights[Mathf.RoundToInt(i-1)].intensity = Mathf.Clamp(curvePos * maxIntensity, minIntensity, maxIntensity);
        }

        if(timer >= 1f+(1f/numOfLights)) { timer = 0f; }
    }
}
