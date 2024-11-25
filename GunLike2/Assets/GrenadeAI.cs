using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAI : MonoBehaviour
{
    public float slowSpd;
    public float fastSpd;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Roll(slowSpd);
        if (Input.GetKey(KeyCode.Space))
        {
            Boost(slowSpd);
        }
    }

    void Roll(float speed)
    {
        rb.AddRelativeTorque(Vector3.right * (100f * speed) * Time.deltaTime, ForceMode.Acceleration);
    }

    void Boost(float speed)
    {
        rb.AddRelativeForce(Vector3.forward * (100f * speed) * Time.deltaTime, ForceMode.Impulse);
    }
}
