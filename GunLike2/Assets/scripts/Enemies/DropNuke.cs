using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropNuke : MonoBehaviour
{
    public GameObject nuke;

    public AudioClip explosion;
    public AudioSource explosionSource;
    public AudioSource whistleSource;
    private void Start()
    {
        float mv = PlayerPrefs.GetFloat("MASTERVOL")/100f;
        float ev = PlayerPrefs.GetFloat("ENEMYVOL")/100f;
        explosionSource.volume = ev * mv;
        whistleSource.volume = ev * mv * 0.3f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject spawnednuke = Instantiate(nuke, transform.position, transform.rotation);
        spawnednuke.GetComponent<NuclearExplosion>().damage = 500;
        explosionSource.clip = explosion; explosionSource.Play(); explosionSource.transform.SetParent(spawnednuke.transform, false);
        Destroy(gameObject);
    }
}
