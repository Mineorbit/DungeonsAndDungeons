using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level currentLevel;

    string name;
    //Temp
    public LevelObjectData floorData;

    
    List<LevelObject> objects;
    Spawn spawn;
    Goal goal;

    void Setup(LevelData.LevelMetaData metaData)
    {
        Clear();
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
                GameObject o = floorObjectData.Create(new Vector3(2*i,-2, 2*j), currentLevel.transform);
                currentLevel.objects.Add(o.GetComponent<LevelObject>());
            }
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
