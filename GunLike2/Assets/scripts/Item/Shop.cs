using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<ItemContainer> sellingCrates;
    public List<ShopCrate> sellingItems;
    public GameObject dome;
    GameObject player;
    PlayerItem pi;
    int membership;
    bool open;
    void Start()
    {
        player = GameObject.Find("Player");
        pi = player.GetComponent<PlayerItem>();
        dome.GetComponent<Animator>().SetBool("Open", true);
    }
    void Update()
    {
        membership = pi.leftItems[112] + pi.rightItems[112];
        if(membership > 0)
        {
            foreach (ShopCrate sc in sellingItems)
            {
                sc.priceModifier = 0.5f / (1.1f * membership);
            }
            foreach (ItemContainer ic in sellingCrates)
            {
                ic.priceModifier = 0.5f / (1.1f * membership);
            }
        }

        if(Vector3.Distance(transform.position, player.transform.position) < 15f)
        {
            dome.GetComponent<Animator>().SetBool("Open", true);
            if (!open) { transform.LookAt(player.transform); }
            transform.localEulerAngles = Vector3.up * transform.localEulerAngles.y;
            open = true;
        }
        else
        {
            dome.GetComponent<Animator>().SetBool("Open", false);
            open = false;
        }
    }
}
