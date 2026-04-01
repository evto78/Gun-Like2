using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    public Image img;
    public Transform flashTransform;
    public List<Sprite> flashSprites;
    public float flashTimer = 0f; float flashDuration = 0.5f;
    void Update()
    {
        flashTimer -= Time.deltaTime/flashDuration;
        img.color = new Color(1, 1, 1, Mathf.Lerp(0, 0.75f, flashTimer));
        flashTransform.localScale = Vector3.one*Mathf.Lerp(1.75f, 1f, flashTimer);
    }
    public void Flash(float intensity)
    {
        img.sprite = flashSprites[Random.Range(0,flashSprites.Count)];
        if (flashTimer < Mathf.Clamp(intensity, 0.33f, 1f)) { flashTimer = intensity; }
        flashTimer = Mathf.Clamp(flashTimer, 0.33f, 1f);
    }
}
