using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level currentLevel;
    public LevelObjectData floorData;

    public LevelData data;
    
    List<LevelObject> objects;
    Spawn spawn;
    Goal goal;

    public void Awake()
    {
        if (currentLevel != null) Destroy(this);
        currentLevel = this;
        objects = new List<LevelObject>();
    }
    public static void Create(LevelData levelData)
    {
        currentLevel.data = levelData;
        Clear();
        Debug.Log(currentLevel);
        CreateGroundPlane(currentLevel.floorData);
    }
   
    static void CreateGroundPlane(LevelObjectData floorObjectData)
    {
        if (floorObjectData == null) return;
           for (int i = -10;i<10;i++)
            for(int j = -10;j<10;j++)
            {
                //Here we need to change to the type of LevelData
                GameObject o = floorObjectData.Create(new Vector3(i,-1, j));
                o.transform.SetParent(currentLevel.transform);
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
