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

            Save("");
        }
        public void Save(string mainPath)
        {

            string path = mainPath + "/gameData/levels/" + ullid.ToString();
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

    public void Load(int ullid)
    {

    }

    public void Save()
    {
        metaData.Save();
        //SaveChangedChunks();

    }
    


    public LevelData(LevelMetaData levelLetaData)
    {
        metaData = levelLetaData;
        chunkMapping = new Dictionary<Tuple<int, int>, int>();
        chunks = new List<Chunk.ChunkData>();
    }
}
