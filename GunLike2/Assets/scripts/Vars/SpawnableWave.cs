using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Wave", menuName = "Spawnable/Create New Wave")]
[System.Serializable]
public class SpawnableWave : ScriptableObject
{
    public List<Spawnable> spawnables;
    public List<float> timeBetween;
    public float additonalDifficultyRequirement;
    public int weight;
    [Header("Filled in Dynamicly (Do Not Fill in Inspector)")]
    public float difficultyReq;
    public float cost;
}
