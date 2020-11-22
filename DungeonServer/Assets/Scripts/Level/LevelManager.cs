using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;


    public LevelData.LevelMetaData[] availableLocalLevels;

    public LevelData.LevelMetaData selectedLevel;
    void Awake()
    {
        if (levelManager != null) Destroy(this);
        levelManager = this;
        selectedLevel = null;
    }

    void OnEnable()
    {
        UpdateLocalLevels();
    }

    public static void UpdateLocalLevels()
    {
        levelManager.availableLocalLevels = GetAllLocalLevels();
    }

    
    public static LevelData.LevelMetaData GetSelectedLevel()
    {
        if(levelManager.availableLocalLevels!=null)
        return levelManager.selectedLevel;

        return null;
    }

    public static LevelData.LevelMetaData SetSelectedLevel(long ulid)
    {
        foreach(LevelData.LevelMetaData d in levelManager.availableLocalLevels)
        {
            if (d.ulid == ulid) levelManager.selectedLevel = d;
        }
        return levelManager.selectedLevel;
    }

    public static void Load(LevelData.LevelMetaData levelData)
    {

        LevelData.LevelMetaData newMetaData = FetchOnline(levelData);
            
        if(levelAvail(levelData))
        {
        Level.Load(levelData);
        }else
        {
            //Cancel and back to lobby
            ServerManager.instance.performAction(ServerManager.GameAction.CancelGame);
        }
    }
    static bool levelAvail(LevelData.LevelMetaData levelToCheck)
    {
        foreach(LevelData.LevelMetaData l in levelManager.availableLocalLevels)
        {
            if(l.ulid == levelToCheck.ulid)
            {
                return true;
            }
        }
        return false;
    }

    public static LevelData.LevelMetaData FetchOnline(LevelData.LevelMetaData levelData)
    {
        if (!levelAvail(levelData))
        {

        int localId = GetFreeUniqueLocalLevelId();

        LevelData.LevelMetaData newLevelData = OnlineLevelOrganizer.onlineLevelOrganizer.FetchLevel(levelData, localId);

        UpdateLocalLevels();
        return newLevelData;
        }
    return levelData;
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
    public static void Delete(LevelData.LevelMetaData levelMetaData)
    {
        FileManager.deleteFolder($"/gameData/levels/{levelMetaData.ullid}");
        UpdateLocalLevels();
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


        Level.Create(levelMetaData);
        Level.Save();
    }


    
}
