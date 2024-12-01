using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    GameObject player;
    public GameObject item;

    public List<Vector4> leftInventory;
    public List<Vector4> rightInventory;

    List<Sprite> itemSprites;

    public GameObject row;
    public List<GameObject> rows;

    private void Start()
    {
        player = GameObject.Find("Player");

        itemSprites = item.GetComponent<Item>().spriteList;
    }

    public void ArrangeInventory()
    {
        
    }
}
