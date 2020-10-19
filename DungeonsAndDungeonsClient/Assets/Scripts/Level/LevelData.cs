using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData
{
    [Serializable]
    public class LevelMetaData
    {


        public LevelMetaData(string lname)
        {
            name = lname;
        }
        public string name;
        public long ulid;
        public int ullid;
        public LevelCreationTemplate creationTemplate;
        public LevelObjectData GetFloorType()
        {
            return creationTemplate.floorType;
        }

        public static LevelMetaData Load(String path)
        {
            //Hier noch safety feature wenn dateien illegal
            path += "/MetaData.json";
            SaveManager saveManager = new SaveManager(SaveManager.StorageType.JSON);
            return saveManager.Load<LevelMetaData>(path);
        }
        public void Save()
        {

            string path = "/gameData/levels/" + ullid.ToString();
            //Save LevelMetaData
            SaveManager saveManager = new SaveManager(SaveManager.StorageType.JSON);
            saveManager.Save(this, path + "/MetaData.json");
        }
    }


    public LevelMetaData metaData;

    Dictionary<Tuple<int, int>, int> chunkMapping;
    List<Chunk.ChunkData> chunks;
    



    public LevelData()
    {
        metaData = new LevelMetaData("Test");
        chunkMapping = new Dictionary<Tuple<int, int>, int>();
        chunks = new List<Chunk.ChunkData>();
    }


    public void Save()
    {
        metaData.Save();
        //Save Index
        SaveManager binStore = new SaveManager(SaveManager.StorageType.BIN);
        string path = "/gameData/levels/" + metaData.ullid.ToString();
        binStore.Save(chunkMapping,path+"/Index.dat");

        foreach(Chunk.ChunkData c in chunks)
        {
            string chunkPath = path + $"/{c.saveID}.dat";
            binStore.Save(c,chunkPath);
        }
    }

    public LevelData(LevelMetaData levelLetaData)
    {
        metaData = levelLetaData;
        chunkMapping = new Dictionary<Tuple<int, int>, int>();
        chunks = new List<Chunk.ChunkData>();
    }
    public LevelData(LevelMetaData levelLetaData, Dictionary<Tuple<int, int>, int> chunkMappings, List<Chunk.ChunkData> chunkDatas )
    {
        metaData = levelLetaData;
        chunkMapping = chunkMappings;
        chunks = chunkDatas;
    }
}
