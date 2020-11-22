using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Level : MonoBehaviour
{
    public static Level currentLevel;

    public LevelData.LevelMetaData levelMetaData;
    //Temp
    public string name;

    public InstantionTarget chunkPrefab;


    Dictionary<Tuple<int, int>, int> chunkLocations;

    List<Chunk> chunks;

    public Spawn[] spawn;

    public Goal goal;

    public bool isLoaded;

    void Setup(LevelData.LevelMetaData metaData)
    {
        Clear();
        levelMetaData = metaData;
        name = metaData.name;

        SetupChunkData();

        chunkPrefab = Resources.Load("pref/level/ChunkPref") as InstantionTarget;

    }

    public void Awake()
    {
        if (currentLevel != null) Destroy(this);
        currentLevel = this;
        spawn = new Spawn[4];
        SetupChunkData();
    }
    public void SetupChunkData()
    {
        chunkLocations = new Dictionary<Tuple<int, int>, int>();
        chunks = new List<Chunk>();
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
        //Check if level exists




        LevelData data = LevelData.Load(levelMetaData);

        currentLevel.InstantiateLevelFromLevelData(data);

        GenerateNavMesh();

        currentLevel.isLoaded = true;
       
    }

    static void GenerateNavMesh()
    {

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


    public int GetSaveID(Tuple<int, int> location)
    {
        int r = 0;
        if (chunkLocations.TryGetValue(location, out r))
        {
            return r;
        }
        else return -1;
    }

    public void SendChunkAt(Vector3 position, int localId)
    {
        Player p = PlayerManager.playerManager.players[localId];
        if (p == null) return;
        //TODO check if player exists
        if (Level.currentLevel != null)
        {
            if (Level.currentLevel.isLoaded)
            {
                Tuple<int, int> chunkLocation = Level.currentLevel.GetChunkLocation(position);
                int saveID = Level.currentLevel.GetSaveID(chunkLocation);
                if (!p.visitedChunks.Contains(saveID))
                {
                    //Send chunk
                    Chunk c = Level.currentLevel.GetChunk(position);
                    if (c != null)
                    {
                        Chunk.ChunkData d = c.GetChunkData(saveID);
                        ChunkDataPacket packet = new ChunkDataPacket(chunkLocation.Item1, chunkLocation.Item2, d);
                        Debug.Log("Sending chunk: "+chunkLocation.Item1+" "+chunkLocation.Item2);
                        Server.SendPacket(localId, packet);
                        p.visitedChunks.Add(saveID);
                    }
                }
            }
        }
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

    public Tuple<int,int> GetChunkLocation(Vector3 position)
    {

        int x = (int)Mathf.Floor(position.x / 32);
        int y = (int)Mathf.Floor(position.z / 32);

         return new Tuple<int, int>(x, y);
        
    }

    public Chunk GetChunk(Vector3 position)
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
