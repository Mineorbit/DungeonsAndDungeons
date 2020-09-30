using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FileStructureProfile", order = 1)]
public class FileStructureProfile : ScriptableObject
{
    public string name;
    public FileStructureProfile[] subStructures;
}
