using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    GameObject player;

    public List<Vector4> leftInventory;
    public List<Vector4> rightInventory;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void ArrangeInventory()
    {
        
    }
}
