using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VelocityTracker : MonoBehaviour
{
    Rigidbody rb;
    public TextMeshProUGUI txt;
    void Start()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = Mathf.Round(rb.velocity.magnitude).ToString();
    }
}
