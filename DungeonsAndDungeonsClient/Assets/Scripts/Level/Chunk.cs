using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class Chunk : MonoBehaviour
{

    public static InstantionTarget chunkObject;
    public List<LevelObject> objects;



    [Serializable]
    public class ChunkData
    {
        public int saveID;
        public List<LevelObject.LevelObjectInstanceData> levelObjectInstanceData;
    }

    public static void Load(ChunkData data, int x, int y)
    {

    }

    void Awake()
    {
        objects = new List<LevelObject>();
    }

    public static void Create(LevelData.LevelMetaData metaData, int x, int y)
    {

    }
    static void Instantiate()
    {

    }

    public void Add(LevelObjectData typeData, Vector3 localPosition)
    {
        if (typeData == null) return;
        GameObject o = typeData.Create(localPosition, transform);
        objects.Add(o.GetComponent<LevelObject>());
    }


    public ChunkData GetChunkData(int saveID)
    {
        ChunkData d = new ChunkData();
        d.saveID = saveID;
        List<LevelObject.LevelObjectInstanceData> instanceData = new List<LevelObject.LevelObjectInstanceData>();

        foreach(LevelObject o in objects)
        {
            instanceData.Add(o.GetLevelObjectInstanceData());
        }

        d.levelObjectInstanceData = instanceData;
        return d;
    }

    public void Remove(LevelObject o)
    {
        objects.Remove(o);
        Destroy(o.gameObject);
    }


    void CreateGroundPlane(LevelObjectData floorObjectData)
    {

        if (floorObjectData == null) return;
        for (int i = 0; i < 32; i++)
            for (int j = 0; j < 32; j++)
            {
                Add(floorObjectData, new Vector3(2 * i, -2, 2 * j));
            }
    }

    void Update()
    {
        
    }
}
