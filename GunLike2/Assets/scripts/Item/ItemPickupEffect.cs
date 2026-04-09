using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupEffect : MonoBehaviour
{
    public List<GameObject> rarities;
    GameObject activeRarity;
    List<Transform> particles = new List<Transform>();
    public AnimationCurve yOffsetCurve;
    Transform tarPos;
    Vector3 startPos;
    float progress;
    bool ready = false;
    public void SetUpEffect(int rarity, Transform targetPos)
    {
        progress = 0;
        rarities[rarity].SetActive(true);
        activeRarity = rarities[rarity];
        for(int i = 0; i < activeRarity.transform.childCount; i++)
        {
            particles.Add(activeRarity.transform.GetChild(i));
        }
        startPos = transform.position;
        tarPos = targetPos;
        ready = true;
    }
    void Update()
    {
        if (!ready) { return; }
        progress += Time.deltaTime*((yOffsetCurve.Evaluate(progress)+0.5f)*3f);
        transform.position = Vector3.Lerp(startPos, tarPos.position, progress) + Vector3.up * yOffsetCurve.Evaluate(progress);
        if (progress < 1) { foreach (Transform t in particles) { t.localScale *= Mathf.Lerp(1f, 0.5f, progress); } }
        else { foreach (Transform t in particles) { t.gameObject.SetActive(false); } }
        if(progress >= 2) { Destroy(gameObject); }
    }
}
