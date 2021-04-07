using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelCreationTemplate", order = 1)]
public class LevelCreationTemplate : ScriptableObject
{
    public LevelObjectData floorType;
    public LevelObjectData wallType;
}
