using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Gun", menuName = "Gun/Create New Gun")]
public class GunObjectData : ScriptableObject
{
    [Header("Info & Visuals")]
    public string gunName;
    public Sprite icon;
    public string descriptionText;
    public List<string> effectText;
    [Header("Functional")]
    public GunType gunType;
    public enum GunType { Pistol, Revolver, BulkFedDoubleBarrel, Vector3, AeroRifle, LittleGun, DaEagle, Crossbow, MutatedKnife, HandCannon, ArcherFish, ShapeChangingGoo}
    public int id;
    public GameObject gunPrefab;
    public GameObject bulletPrefab;
    [Header("Base Stats")]
    public float baseMagSize;
    public float baseAtkSpd;
    public float baseReSpd;
    public float baseBulSpd;
    public float baseDmg;
    public float baseAcc;
    public float baseBulSize;
    public int baseBulPir;
    public float baseCritChance;
    public float baseCritDamage;
    public float baseWeakPointChance;
    public float baseWeakPointDamage;
    [Header("Sounds")]
    public List<AudioClip> shootClips;
    public List<AudioClip> reloadClips;
    public List<AudioClip> noAmmoClips;
}
