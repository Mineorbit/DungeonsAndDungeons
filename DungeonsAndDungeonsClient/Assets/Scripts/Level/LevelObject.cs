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
        public LevelObjectInstanceData(Vector3 loc, Quaternion rot, int data)
        {
            objectData = data;
            location = new float[3];
            location[0] = loc.x;
            location[1] = loc.y;
            location[2] = loc.z;
            rotation = new float[4];
            rotation[0] = rot.x;
            rotation[1] = rot.y;
            rotation[2] = rot.z;
            rotation[3] = rot.w;
        }
        public int objectData;
        public float[] location;
        public float[] rotation;
        //LevelObjectData typeData
    }


    public int ObjectDataID;
    public Chunk chunk;

    public LevelObjectInstanceData GetLevelObjectInstanceData()
    {
        LevelObjectInstanceData d =  new LevelObjectInstanceData(transform.position,transform.rotation,ObjectDataID);
        return d;
    }

    UnityEvent actionEvent;


    public virtual void OnCreate()
    {
        Level.testRoundStart.AddListener(OnTestRoundStart);
    }

    public virtual void OnTestRoundStart()
    {

    }

    public virtual void OnDestroy()
    {
        Level.testRoundStart.AddListener(OnTestRoundStart);
        if (chunk!=null)
        chunk.objects.Remove(this);
    }


    public virtual void Action()
    {
        actionEvent.Invoke();
    }
}
