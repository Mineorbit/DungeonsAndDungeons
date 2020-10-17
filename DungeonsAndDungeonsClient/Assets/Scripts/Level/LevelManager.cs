using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;


    public LevelData.LevelMetaData[] availableLocalLevels;

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
            { 
            data.Add(levelMetaData);
            }
        }
        return data.ToArray();
    }
    public void Delete()
    {

    }
    static int GetFreeUniqueLocalLevelId()
    {

        UpdateLocalLevels();
        int id = 0;
        List<int> ids = new List<int>();
        foreach (LevelData.LevelMetaData d in levelManager.availableLocalLevels)
        {
            ids.Add(d.ullid);
        }
        while (ids.Contains(id))
        {
            id++;
        }
        return id;
    }
    public static void New(LevelData.LevelMetaData levelMetaData)
    {
        //Setup Folder
        levelMetaData.ullid = LevelManager.GetFreeUniqueLocalLevelId();

        string path = "/gameData/levels/" + levelMetaData.ullid.ToString();
        Debug.Log("Test");
        FileManager.createFolder(path);

        levelMetaData.Save();

        Level.Create(levelMetaData);
    }


    
}
