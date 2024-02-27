using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/ Recipe")]
public class Recipe : ScriptableObject
{
    public Item[] ingredients;
    public int[] ingredient_count;

    public Item result;
    public int result_count;
    [Tooltip("In Seconds")]
    public float timeToCraft;
}
