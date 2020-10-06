using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public LevelData[] levels;

    public bool levelLoaded;

    void Awake()
    {
        if (levelManager != null) Destroy(this);
        levelManager = this;
    }
    public void Load(long ulid)
    {

    }
    public void Load(int index)
    {

    }
    public void Delete()
    {

    }
    public static void New(LevelData.LevelMetaData levelMetaData)
    {
        LevelData levelData = new LevelData(levelMetaData);
        Level.Create(levelData);
    }
    
}
