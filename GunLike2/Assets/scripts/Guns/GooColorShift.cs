using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooColorShift : MonoBehaviour
{
    List<SmartMeshRen> mrs = new List<SmartMeshRen>();
    public float speed;
    public float randomness;
    Vector3 dir = Vector3.one;
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
            r += dir.x*(Random.Range(1f, randomness) * Time.deltaTime * speed)/255f; if (r > 1) { dir.x = -1; } else if (r < 0) { dir.x = 1; }
            g += dir.y*(Random.Range(1f, randomness) * Time.deltaTime * speed)/255f; if (g > 1) { dir.y = -1; } else if (g < 0) { dir.y = 1; }
            b += dir.z*(Random.Range(1f, randomness) * Time.deltaTime * speed)/255f; if (b > 1) { dir.z = -1; } else if (b < 0) { dir.z = 1; }
            smr.mr.material.color = new Color(r, g, b, smr.matColor.a);
            smr.mat = smr.mr.material; smr.matColor = smr.mr.material.color;
        }
    }
}
