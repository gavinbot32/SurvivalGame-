using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotTag { None, Head, Chest, Legs, Feet}

[CreateAssetMenu(menuName = "Scriptable Objects/ Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public SlotTag itemTag;
    public bool stackable;
    public bool equipable;

    [Header("If item can be equipped")]
    public GameObject equipmentPrefab;
}
