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
        public enum Template {Regular , Sand, Boss };
        public Template template;

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

            string path = mainPath+"/gameData/levels/" + ullid.ToString();
            //Save LevelMetaData
            SaveManager saveManager = new SaveManager(SaveManager.StorageType.JSON);
            saveManager.Save(this, path + "/MetaData.json");
        }
    }
    public LevelMetaData metaData;

    public LevelObjectData[] levelObjectData;
    public LevelData()
    {
        levelObjectData = new LevelObjectData[1000];
    }

    public void Save()
    {
        metaData.Save();

    }
    public LevelData(LevelMetaData levelLetaData)
    {
        metaData = levelLetaData;
        levelObjectData = new LevelObjectData[1000];
    }
}
