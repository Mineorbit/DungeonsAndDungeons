using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level currentLevel;

    LevelData.LevelMetaData levelMetaData;
    //Temp
    public string name;

    public InstantionTarget chunkPrefab;


    Dictionary<Tuple<int, int>, int> chunkLocations;
    List<Chunk> chunks;
    Spawn spawn;
    Goal goal;

    void Setup(LevelData.LevelMetaData metaData)
    {
        Clear();
        levelMetaData = metaData;
        name = metaData.name;

        chunkLocations = new Dictionary<Tuple<int, int>, int>();
        chunks = new List<Chunk>();

        chunkPrefab = Resources.Load("pref/level/ChunkPref") as InstantionTarget;

    }

    public void Awake()
    {
        if (currentLevel != null) Destroy(this);
        currentLevel = this;
    }

    public static void Create(LevelData.LevelMetaData levelMetaData)
    {
        currentLevel.Setup(levelMetaData);
        //Save right after create
    }

    public static void Save()
    {

    }

    public static void Load(int ullid)
    {

    }

    public static void Load(LevelData levelData)
    {
        currentLevel.Setup(levelData.metaData);
    }

    public Chunk AddChunk(Tuple<int, int> location)
    {

        Chunk c = InstantiateChunk(location.Item1,location.Item2);
        chunks.Add(c);
        chunkLocations.Add(location,chunks.Count-1);
        return c;
    }

    public Chunk InstantiateChunk(int x, int y)
    {
        GameObject chunkObject = chunkPrefab.Create(new Vector3(x*32f,0,y*32f),transform);
        return chunkObject.GetComponent<Chunk>();
    }

    public void LoadChunk(Chunk.ChunkData chunkData)
    { 
    }

    public void Add(LevelObjectData typeData, Vector3 position)
    {
        Chunk targetChunk = GetChunk(position);
        if(targetChunk == null)
        {
         targetChunk =  AddChunk(GetChunkLocation(position));
        }
        targetChunk.Add(typeData,position);
      //  GameObject o = typeData.Create(position, currentLevel.transform);
      //  currentLevel.objects.Add(o.GetComponent<LevelObject>());
    }

    public void Add(LevelObjectData typeData, Vector3 position, Vector3 normal)
    {
       // GameObject o = typeData.Create(position, currentLevel.transform);
       // currentLevel.objects.Add(o.GetComponent<LevelObject>());
    }
    Tuple<int,int> GetChunkLocation(Vector3 position)
    {

        int x = (int)Mathf.Floor(position.x / 32);
        int y = (int)Mathf.Floor(position.z / 32);

         return new Tuple<int, int>(x, y);
        
    }
    Chunk GetChunk(Vector3 position)
    {
        int chunkId;
        Tuple<int, int> loc = GetChunkLocation(position);
        if (chunkLocations.TryGetValue(loc,out chunkId))
        {
            return chunks[chunkId];
        }else
        {
            return null;
        }
    }
    public void Remove(LevelObject o)
    {
        //objects.Remove(o);
        //Destroy(o.gameObject);
    }

    public static void Clear()
    {
        foreach(Transform child in currentLevel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public static void Destroy()
    {
        Clear();
        currentLevel = null;
    }

}
