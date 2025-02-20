using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomID;
    public List<GameObject> doors;
    public bool isMainPath;
    public BoxCollider interCollider;
}
