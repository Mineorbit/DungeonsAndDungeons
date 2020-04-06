using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelObjectData
{
    public GameManager.Selectable type;
    public float[] location;
    public LevelObject.Orientation orientation;
}
