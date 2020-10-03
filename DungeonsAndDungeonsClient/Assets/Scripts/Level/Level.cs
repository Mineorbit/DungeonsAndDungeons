using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level currentLevel;

    public LevelData data;

    LevelObject[] objects;
    Spawn spawn;
    Goal goal;
    public Level(LevelData levelData)
    {
        data = levelData;
    }

    public static void Create(LevelData levelData)
    {
        Level level = new Level(levelData);
        currentLevel = level;
        currentLevel.InstantiateFromLevelData();
    }
    public static void Create()
    {
        Create(null);
    }
    public void InstantiateFromLevelData()
    {
        Clear();
    }
    public void Clear()
    {
        foreach(Transform child in transform)
        {
            Destroy(child);
        }
        currentLevel = null;
    }

}
