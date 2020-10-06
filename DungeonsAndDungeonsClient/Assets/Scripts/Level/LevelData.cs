using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public class LevelMetaData
    {
        public LevelMetaData(string name)
        {

        }
        string name;
        long ulid;
        enum Template {Regular , Sand, Boss };
        Template template;
    }
    LevelMetaData metaData;
    LevelObjectData[] levelObjectData;
    public LevelData()
    {
        levelObjectData = new LevelObjectData[1000];
    }
    public LevelData(LevelMetaData levelLetaData)
    {
        metaData = levelLetaData;
        levelObjectData = new LevelObjectData[1000];
    }
}
