using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerPlaceData
{
    public Terrain myTerrain;
    public Vector3 worldPos = Vector3.zero;
    public Vector3 localPos = Vector3.zero;
    public Vector2 arrayPos = Vector2.zero;
    public float height = 0;
    public bool placeable = false;
    public bool onRoad = false;
}
