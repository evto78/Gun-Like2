using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StickyNote : MonoBehaviour
{
    public List<string> jokes;
    string joke;
    public TextMeshProUGUI txt;
    Rigidbody rb;
    Collider myCollider;
    public AnimationCurve curve;
    float lifetimer;
    float ignoreCollisionTimer;
    bool collided;
    void Start()
    {
        lifetimer = 0f;
        ignoreCollisionTimer = 20f;
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponentInChildren<Collider>();
        joke = jokes[Random.Range(0, jokes.Count)];
        txt.text = joke;
        Destroy(gameObject, 20f);
        rb.AddForce((Vector3.up*2f + new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))/2f)*2f);
        rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))*2f);
        transform.localEulerAngles = new Vector3(-10, Random.Range(0, 360), 0);
        transform.localScale = Vector3.one * curve.Evaluate(Mathf.Clamp(lifetimer, 0, 1));
    }
    private void Update()
    {
        lifetimer += Time.deltaTime; transform.localScale = Vector3.one * curve.Evaluate(Mathf.Clamp(lifetimer, 0, 1));
        ignoreCollisionTimer -= Time.deltaTime; if (ignoreCollisionTimer <= 0) { Destroy(rb); Destroy(myCollider); Destroy(this); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Untagged" || collision.collider.gameObject.tag == "Ground")
        {
            if (collided) { return; }
            collided = true; ignoreCollisionTimer = 2f;
        }
    }
}
