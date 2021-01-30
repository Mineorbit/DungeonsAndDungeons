using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Level : MonoBehaviour
{
    public static Level currentLevel;

    public LevelData.LevelMetaData levelMetaData;
    //Temp
    public string name;

    public InstantionTarget chunkPrefab;

    List<LevelObject> dynamicObjects;


    Dictionary<Tuple<int, int>, int> chunkLocations;
    
    public List<Chunk> chunks;

    public Spawn[] spawn;

    public Goal goal;


    public static LevelData lastData;

    public static UnityEvent testRoundStart;
    public static UnityEvent playRoundStart;

 

    Enemy[] enemies;
    void Setup(LevelData.LevelMetaData metaData)
    {

        Clear();
        levelMetaData = metaData;
        name = metaData.name;

        SetupChunkData();
    }

    void SetupEvents()
    {
        testRoundStart = new UnityEvent();
        playRoundStart = new UnityEvent();

        Level.testRoundStart.AddListener(LevelNavGenerator.UpdateNavMesh);
    }
    public static void SetupTestRound()
    {
        ChunkManager.ActivateAllChunks();
        testRoundStart.Invoke();
    }

    public static void SetupPlayRound()
    {
        ChunkManager.ActivateAllChunks();
        playRoundStart.Invoke();
    }


    public static Enemy[] GetAllEnemies()
    {
        if(currentLevel.enemies  == null) currentLevel.enemies = currentLevel.gameObject.GetComponentsInChildren<Enemy>();
        return currentLevel.enemies;
    }


    void SetupChunkData()
    {
        chunkLocations = new Dictionary<Tuple<int, int>, int>();
        chunks = new List<Chunk>();
        chunkPrefab = Resources.Load("pref/level/ChunkPref") as InstantionTarget;
    }

    public void Awake()
    {
        if (currentLevel != null) Destroy(this);
        currentLevel = this;

        spawn = new Spawn[4];

        SetupEvents();
        SetupChunkData(); 
    }

    public static void Create(LevelData.LevelMetaData levelMetaData)
    {

        currentLevel.Setup(levelMetaData);

    }

    public static void Save()
    {
        ChunkManager.ActivateAllChunks();
        string path = "/gameData/levels/" + currentLevel.levelMetaData.ullid.ToString();
        Debug.Log("Saving Level: " + path);

        FileManager.createFolder(path);

        LevelData data = currentLevel.GetLevelData();
        data.Save();
    }

    public static void Load(LevelData.LevelMetaData levelMetaData)
    {

        LevelData data = LevelData.Load(levelMetaData);
        LoadFromLevelData(data);

    }

    public static void LoadFromLevelData(LevelData data)
    {

        currentLevel.InstantiateLevelFromLevelData(data);
        storeCache();

    }

    void InstantiateLevelFromLevelData(LevelData d)
    {
        currentLevel.Setup(d.metaData);
        //Load Index
        chunkLocations = d.chunkMapping;
        //currentLevel.
        //Load Chunks for every value of index
        foreach(Chunk.ChunkData c in d.chunks)
        {
            Chunk chunk = FromChunkData(c);
        }
    }

    public Chunk FromChunkData(Chunk.ChunkData chunkData,Tuple<int,int> location)
    {
        Chunk c = InstantiateChunk(location.Item1, location.Item2);
        c.Instantiate(chunkData);
        chunks.Add(c);

        return c;
    }

    Chunk FromChunkData(Chunk.ChunkData chunkData)
    {
        Tuple<int,int> location = chunkLocations.FirstOrDefault(x => x.Value == chunkData.saveID).Key;
        return FromChunkData(chunkData,location);
    }

    public static List<Chunk> GetChunks()
    {
        return currentLevel.chunks;
    }

    List<Chunk.ChunkData> GetChunkDatas()
    {
        List<Chunk.ChunkData> chunkDatas = new List<Chunk.ChunkData>();
        for(int i = 0;i<chunks.Count;i++)
        {
            Chunk.ChunkData d = chunks[i].GetChunkData(i);
            chunkDatas.Add(d);
        }
        return chunkDatas;
    }

    public LevelData GetLevelData()
    {
        LevelData d = new LevelData(levelMetaData,this.chunkLocations,GetChunkDatas());
        return d;
    }

    public void LoadChunk(Chunk.ChunkData chunkData, Tuple<int,int> location)
    {
        Chunk c = InstantiateChunk(location.Item1,location.Item2);
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


    public void Add(LevelObjectData typeData, Vector3 position)
    {
        Debug.Log("Test");
        AddObject(typeData, position, new Quaternion(0,0,0,0));
        storeCache();
    }

    public void Add(LevelObjectData typeData, Vector3 position, Quaternion rotation)
    {

        Debug.Log("Moin");
        AddObject(typeData,position,rotation);
        Debug.Log("Moin2");
        storeCache();
    }

    public void AddDynamic(LevelObjectData typeData, Vector3 position, Quaternion rotation)
    {
        LevelObject levelObject = AddObject(typeData, position, rotation);
        if(levelObject != null)
        dynamicObjects.Add(levelObject);
    }

    public LevelObject AddObject(LevelObjectData typeData, Vector3 position, Quaternion rotation)
    {
        Chunk targetChunk = GetChunk(position);
        if (targetChunk == null)
        {
            targetChunk = AddChunk(GetChunkLocation(position));
        }
        return targetChunk.Add(typeData, position, rotation);
    }

    public static void storeCache()
    {
        lastData = currentLevel.GetLevelData();
    }





    public void Add(LevelObjectData typeData, Vector3 position, Vector3 normal)
    {
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
        if(o != null)
        o.chunk.Remove(o);

        storeCache();
    }
    public void RemoveObject(LevelObject o)
    {
        if (o != null)
            o.chunk.Remove(o);
    }


    public static void Clear()
    {
        currentLevel.goal = null;
        for (int i = 0; i < 4; i++) currentLevel.spawn[i] = null;
        foreach(Transform child in currentLevel.transform)
        {
            Destroy(child.gameObject);
        }
    }


    public static void Reset()
    {
        Debug.Log("Resetting Level");
        Clear();
        LoadFromLevelData(lastData);
    }

    public static void Destroy()
    {
        Clear();
        currentLevel = null;
    }

}
