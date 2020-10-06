using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelObjectData", order = 1)]
public class LevelObjectData : InstantionTarget
{
    public string FullName;
    public Vector3 Scale;
}
