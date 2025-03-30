using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteCycles : MonoBehaviour
{
    public Image img;
    public List<Sprite> sprites;
    public float animSpeed;
    int curSprite;
    float animTimer;

    public bool idle;
    void Start()
    {
        img = GetComponent<Image>();

        img.sprite = sprites[0];
        curSprite = 0;
        animTimer = Random.Range(0, animSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (idle)
        {
            animTimer -= Time.deltaTime * Random.Range(0.8f, 1.2f);
            if (animTimer < 0) { animTimer = animSpeed; curSprite++; }
            if (curSprite >= sprites.Count) { curSprite = 0; }
            img.sprite = sprites[curSprite];
        }
    }
}
