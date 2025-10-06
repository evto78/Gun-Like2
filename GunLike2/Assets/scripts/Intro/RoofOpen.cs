using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofOpen : MonoBehaviour
{
    Animator anim;
    public List<Light> lights;
    List<float> lightIntensities;
    public bool playIntro;
    float timer;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("ForceOpen");
        lightIntensities = new List<float>();
        foreach(Light l in lights)
        {
            lightIntensities.Add(l.intensity);
        }
        if (playIntro)
        {
            anim.SetTrigger("ForceClose");
            foreach (Light l in lights)
            {
                l.intensity = 0;
            }
        }
    }
    public void OpenRoof()
    {
        StartCoroutine(openSequence());
    }
    private IEnumerator openSequence()
    {
        anim.SetTrigger("ForceClose");
        anim.SetTrigger("Open");
        float i = 0;
        while(i < 1)
        {
            i += Time.deltaTime / 10f;
            for(int y = 0; y < lightIntensities.Count; y++)
            {
                lights[y].intensity = Mathf.Lerp(0f, lightIntensities[y], i);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
