using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    public int rarity;
    public float speedMAX;
    public float speedMIN;
    float speed;
    public FishMinigameGAME manager;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMIN, speedMAX);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(transform.forward.x, 0f, transform.forward.z) * speed * Time.deltaTime;
        if(transform.localPosition.x > 0.65f || transform.localPosition.x < -0.65f || transform.localPosition.z > 0.6f || transform.localPosition.z < -0.85f)
        {
            //Debug.Log("Goodbye cruel world: "+transform.localPosition);
            Destroy(gameObject);
        }
    }
    private void OnMouseDown()
    {
        manager.FishCaught(rarity);
        Destroy(gameObject);
    }
}
