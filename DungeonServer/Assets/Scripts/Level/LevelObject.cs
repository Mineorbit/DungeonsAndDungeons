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
        public LevelObjectInstanceData(Vector3 loc, int data)
        {
            objectData = data;
            location = new float[3];
            location[0] = loc.x;
            location[1] = loc.y;
            location[2] = loc.z;
        }
        public int objectData;
        public float[] location;
        //LevelObjectData typeData
    }

    public int ObjectDataID;

    public LevelObjectInstanceData GetLevelObjectInstanceData()
    {
        LevelObjectInstanceData d =  new LevelObjectInstanceData(transform.position,ObjectDataID);
        return d;
    }
    UnityEvent actionEvent;
    public void Awake()
    {

    }
    public virtual void OnCreate()
    {

    }
    public virtual void OnDestroy()
    {
        Level.currentLevel.Remove(this);
    }
    public void Action()
    {
        actionEvent.Invoke();
    }
}
