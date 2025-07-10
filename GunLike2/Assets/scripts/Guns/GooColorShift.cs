using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooColorShift : MonoBehaviour
{
    List<SmartMeshRen> mrs = new List<SmartMeshRen>();
    public float speed;
    public float randomness;
    private void Start()
    {
        if(TryGetComponent<MeshRenderer>(out MeshRenderer gmr))
        {
            SmartMeshRen temp = new SmartMeshRen();
            temp.mr = gmr;
            temp.mat = gmr.material;
            temp.matColor = gmr.material.color;
            mrs.Add(temp);
        }
        foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            SmartMeshRen temp = new SmartMeshRen();
            temp.mr = mr;
            temp.mat = mr.material;
            temp.matColor = mr.material.color;
            mrs.Add(temp);

        }
        foreach (SmartMeshRen smr in mrs)
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            smr.mr.material.color = new Color(r, g, b, smr.matColor.a);
            smr.mat = smr.mr.material; smr.matColor = smr.mr.material.color;
        }
    }
    void Update()
    {
        foreach(SmartMeshRen smr in mrs)
        {
            float r = smr.matColor.r;
            float g = smr.matColor.g;
            float b = smr.matColor.b;
            r += (Random.Range(1f, randomness) * Time.deltaTime * speed)/255f; if (r > 1) { r = (Random.Range(1f, randomness) * Time.deltaTime * speed) / 255f; }
            g += (Random.Range(1f, randomness) * Time.deltaTime * speed)/255f; if (g > 1) { g = (Random.Range(1f, randomness) * Time.deltaTime * speed) / 255f; }
            b += (Random.Range(1f, randomness) * Time.deltaTime * speed)/255f; if (b > 1) { b = (Random.Range(1f, randomness) * Time.deltaTime * speed) / 255f; }
            smr.mr.material.color = new Color(r, g, b, smr.matColor.a);
            smr.mat = smr.mr.material; smr.matColor = smr.mr.material.color;
        }
    }
}
