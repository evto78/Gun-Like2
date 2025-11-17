using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effects/Create New Effect")]
[System.Serializable]
public class EffectObject : ScriptableObject
{
    [Header("Functional")]
    public int id;
    [Tooltip("What the effect will be displayed as to the player")]
    public string displayName;
    public Sprite icon;
    [Tooltip("How long until 1 stack of this item is removed. Make negative if effect does not decay over time")]
    public float decayTime;
    [Tooltip("Type can be -1, 0, or 1. For Negative, Nutrual, and Positive")]
    public int type;
}
