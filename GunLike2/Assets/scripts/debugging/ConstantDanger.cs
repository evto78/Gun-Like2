using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDanger : MonoBehaviour
{
    UIManager uiman;
    // Start is called before the first frame update
    void Start()
    {
        uiman = GameObject.Find("Player").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        uiman.AddDangerSource(transform, transform.position, false);
    }
}
