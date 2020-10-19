using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class LevelObject : MonoBehaviour
{
    [Serializable]
    public class LevelObjectInstanceData
    {
        public LevelObjectInstanceData(Vector3 loc)
        {
            location = new float[3];
            location[0] = loc.x;
            location[1] = loc.y;
            location[2] = loc.z;
        }
        public float[] location;
        //LevelObjectData typeData
    }

    public LevelObjectInstanceData GetLevelObjectInstanceData()
    {
        LevelObjectInstanceData d =  new LevelObjectInstanceData(transform.position);
        return d;
    }
    UnityEvent actionEvent;
    public void Awake()
    {

    }
    public void Action()
    {
        actionEvent.Invoke();
    }
}
