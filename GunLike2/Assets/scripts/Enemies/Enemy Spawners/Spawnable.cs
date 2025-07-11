using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawnable", menuName = "Spawnable/Create New Spawnable")]
[System.Serializable]
public class Spawnable : ScriptableObject
{
    public GameObject thingToSpawn;
    public float difficultyRequirement;
    public Vector2 amountToSpawn;
    public float pointCost;
    public enum Type { ground, air, walker}
    public Type type;
}
