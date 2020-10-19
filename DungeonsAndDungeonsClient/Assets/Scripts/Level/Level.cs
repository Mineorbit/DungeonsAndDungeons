using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

    }

    public static void Save()
    {
        string path = "/gameData/levels/" + currentLevel.levelMetaData.ullid.ToString();
        Debug.Log("Saving Level: " + path);
        FileManager.createFolder(path);

        LevelData data = currentLevel.GetLevelData();
        data.Save();
    }
    public static void Load(LevelData.LevelMetaData levelMetaData)
    {

        LevelData data = LevelData.Load(levelMetaData);
        currentLevel.InstantiateLevelFromLevelData(data);
       
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
            chunks.Add(chunk);
        }
    }


    Chunk FromChunkData(Chunk.ChunkData chunkData)
    {
        Tuple<int,int> location = chunkLocations.FirstOrDefault(x => x.Value == chunkData.saveID).Key;
        Chunk c = InstantiateChunk(location.Item1, location.Item2);
        c.Instantiate(chunkData);
        return c;
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
        if(o != null)
        GetChunk(o.transform.position).Remove(o);
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
