using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level currentLevel;

    LevelData.LevelMetaData levelMetaData;
    //Temp
    public LevelObjectData floorData;
    public string name;
    
    List<LevelObject> objects;
    Spawn spawn;
    Goal goal;

    void Setup(LevelData.LevelMetaData metaData)
    {
        Clear();
        levelMetaData = metaData;
        name = metaData.name;
    }
    public void Awake()
    {
        if (currentLevel != null) Destroy(this);
        currentLevel = this;
        objects = new List<LevelObject>();
    }
    public static void Create(LevelData.LevelMetaData levelMetaData)
    {
        currentLevel.Setup(levelMetaData);
        CreateGroundPlane(currentLevel.floorData);
        //Save right after create
    }
    public static void Save()
    {

    }
    public static void Load(LevelData levelData)
    {
        currentLevel.Setup(levelData.metaData);
    }
    static void CreateGroundPlane(LevelObjectData floorObjectData)
    {

        if (floorObjectData == null) return;
           for (int i = -10;i<10;i++)
            for(int j = -10;j<10;j++)
            {
                //Here we need to change to use the Template in LevelData
                currentLevel.Add(floorObjectData, new Vector3(2 * i, -2, 2 * j));


            }
    }


    public void Add(LevelObjectData typeData,Vector3 position)
    {
        GameObject o = typeData.Create(position, currentLevel.transform);
        currentLevel.objects.Add(o.GetComponent<LevelObject>());
    }
    public void Add(LevelObjectData typeData, Vector3 position, Vector3 normal)
    {
        GameObject o = typeData.Create(position, currentLevel.transform);
        currentLevel.objects.Add(o.GetComponent<LevelObject>());
    }
    public void Remove(LevelObject o)
    {
        objects.Remove(o);
        Destroy(o.gameObject);
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
