using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Catchable", menuName = "ScriptableObjects/Catchables", order = 1)]
public class BaseCatchable : ScriptableObject
{
    public string CatchableName;
    public GameObject CatchableObj;
    public bool IsJunk;
    public double catchChance;
}

