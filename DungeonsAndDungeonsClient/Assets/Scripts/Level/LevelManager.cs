using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;


    public LevelData.LevelMetaData[] availableLocalLevels;
    static int freeLocalLevelId;

    void Awake()
    {
        if (levelManager != null) Destroy(this);
        levelManager = this;
    
    }
    void OnEnable()
    {
        UpdateLocalLevels();
    }


    static void UpdateLocalLevels()
    {
        levelManager.availableLocalLevels = GetAllLocalLevels();
    }
    public void Load(long ulid)
    {

    }
    public static LevelData.LevelMetaData[] GetAllLocalLevels()
    {
       string[] levels = Directory.GetDirectories(FileManager.GetLevelPath());
        List<LevelData.LevelMetaData> data = new List<LevelData.LevelMetaData>();
        foreach(string path in levels)
        {
            LevelData.LevelMetaData levelMetaData = LevelData.LevelMetaData.Load(path);
            if(levelMetaData!=null)
            data.Add(levelMetaData);
        }
        return data.ToArray();
    }
    public void Delete()
    {

    }
    static int GetFreeUniqueLocalLevelId()
    {
        int id = 0;

        foreach(LevelData.LevelMetaData data in levelManager.availableLocalLevels)
        {
            if(id==data.ullid)
            {
                id++;
            }
        }
        return id;
    }
    public static void New(LevelData.LevelMetaData levelMetaData)
    {
        UpdateLocalLevels();
        //Setup Folder
        levelMetaData.ullid = LevelManager.GetFreeUniqueLocalLevelId();

        string path = "/gameData/levels/" + levelMetaData.ullid.ToString();
        FileManager.createFolder(path);

        levelMetaData.Save();

        Level.Create(levelMetaData);
    }


    
}
