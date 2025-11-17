using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawnable", menuName = "Spawnable/Create New Spawnable")]
[System.Serializable]
public class Spawnable : ScriptableObject
{
    [Header("Functional")]
    public string enemyName;
    public enum Type { ground, air, walker }
    public Type type;
    [Header("Faction")]
    public bool isMechanical;
    public bool isMutated;
    public bool isGhostly;
    public bool isSmall;
    [Header("Spawning")]
    public GameObject thingToSpawn;
    public float difficultyRequirement;
    public Vector2 amountToSpawn;
    public float pointCost;
    
}
