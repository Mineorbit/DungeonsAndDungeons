using System.Collections;
using System;
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
        public static ChunkData FromBytes(byte[] data)
        {
            return null;
        }
    }

    public static void Load(ChunkData data, int x, int y)
    {

    }
   
    public static (Dictionary<Tuple<int, int>, int> chunkMappings , List<Chunk.ChunkData> chunks) LoadChunkData(LevelData.LevelMetaData data)
    {
        SaveManager binManager = new SaveManager(SaveManager.StorageType.BIN);

        List<ChunkData> chunkData = new List<ChunkData>();
        Dictionary<Tuple<int, int>, int> loadChunkMapping = binManager.Load<Dictionary<Tuple<int, int>, int>>(Application.persistentDataPath+ $"/gameData/levels/{data.ullid}/Index.dat");


        foreach(int i in loadChunkMapping.Values)
        {
        chunkData.Add(binManager.Load<ChunkData>(Application.persistentDataPath+ $"/gameData/levels/{data.ullid}/{i}.dat"));
        }
        return (loadChunkMapping, chunkData);
    }

    LevelObjectData[] objectDataPrototypes;
    void Awake()
    {
        objectDataPrototypes = Resources.LoadAll<LevelObjectData>("pref/level/data");
        objects = new List<LevelObject>();
    }

    public static void Create(LevelData.LevelMetaData metaData, int x, int y)
    {

    }

    LevelObjectData GetByID(int id)
    {
        foreach(LevelObjectData d in objectDataPrototypes)
        {
            if (d.ID == id) return d;
        }
        return null;
    }

    public void Instantiate(ChunkData chunkData)
    {
        foreach(LevelObject.LevelObjectInstanceData instanceObj in chunkData.levelObjectInstanceData)
        {
            Vector3 position = new Vector3(instanceObj.location[0], instanceObj.location[1], instanceObj.location[2]);
            Quaternion rotation = new Quaternion(instanceObj.rotation[0], instanceObj.rotation[1], instanceObj.rotation[2], instanceObj.rotation[3]);
            int typeID = instanceObj.objectData;
            LevelObjectData d = GetByID(typeID);
            Add(d, position, rotation);
        }
    }

    public LevelObject Add(LevelObjectData typeData, Vector3 localPosition, Quaternion localRotation)
    {
        if (typeData == null) return null;
        GameObject o = typeData.Create(localPosition, localRotation, transform);
        objects.Add(o.GetComponent<LevelObject>());
        return o.GetComponent<LevelObject>();
    }

    public LevelObject Add(LevelObjectData typeData, Vector3 localPosition)
    {
        if (typeData == null) return null;
        GameObject o = typeData.Create(localPosition, transform);
        objects.Add(o.GetComponent<LevelObject>());
        return o.GetComponent<LevelObject>();
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
