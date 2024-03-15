using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public List<int> playerItems;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "item")
        {
            playerItems[collision.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
            Debug.Log("Item of Item ID " + collision.gameObject.GetComponentInParent<Item>().WhatItem() + " added to inventory");
            Destroy(collision.gameObject);
        }
    }
}
