using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.IO.Compression;
using System.IO;

public class OnlineLevelOrganizer : MonoBehaviour
{
    [Serializable]
    class ReceiveLevelMetaData
    {
        public long ULId;
        public int Template;
        public string Description;
        public string CreationDate;
        public int GlobalId;
        public string Name;
        public LevelData.LevelMetaData GetLevelMetaData()
        {
            LevelData.LevelMetaData data = new LevelData.LevelMetaData(Name);
            data.ulid = ULId;
            data.creationDate = CreationDate;
            data.description = Description;
            return data;
        }
    }
    [Serializable]
    class FetchedLevels
    {
        public ReceiveLevelMetaData[] levels;
    }

    public static OnlineLevelOrganizer onlineLevelOrganizer;

    LevelData.LevelMetaData[] levels;

    public void Start()
    {
        if (onlineLevelOrganizer != null) Destroy(this);
        onlineLevelOrganizer = this;
        FetchPopularLevels();
    }

    public void FetchPopularLevels()
    {
        Debug.Log("Fetching List of online Levels");
        StartCoroutine("FetchList");
    }

    


    public LevelData.LevelMetaData FetchLevel(LevelData.LevelMetaData metaData, int newlocalId)
    {

        if (metaData == null) return null;





        long ulid = metaData.ulid;

        string url = $"http://www.josch557.xyz:13337/pull?ulid={ ulid }";

        string path = Application.persistentDataPath + $"/gameData/c_levels/g{ulid}.zip";


        if (File.Exists(path))
        {
            File.Delete(path);
        }


        WebClient client = new WebClient();

        client.DownloadFile(url, path);

        string unzipPath = Application.persistentDataPath + $"/gameData/levels/{newlocalId}";

        ZipFile.ExtractToDirectory(path, unzipPath);


        LevelData.LevelMetaData d = LevelData.LevelMetaData.Load(unzipPath);
        d.ullid = newlocalId;
        d.Save();

        return d;
    }

    public LevelData.LevelMetaData GetTopLevel()
    {
        long i = 0;
        LevelData.LevelMetaData r = null;
        if (levels == null) return null;
        foreach(LevelData.LevelMetaData levelMetaData in levels)
        {
            if(i < levelMetaData.ulid)
            {
                i = levelMetaData.ulid;
                r = levelMetaData;
            }
        }
        return r;
    }

   

    IEnumerator FetchList()
    {

        UnityWebRequest www = UnityWebRequest.Get("http://www.josch557.xyz:13337/gameServerLoad");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonText = www.downloadHandler.text;
            FetchedLevels result = JsonUtility.FromJson<FetchedLevels>("{\"levels\":" + jsonText + "}");
            List<LevelData.LevelMetaData> newLevels = new List<LevelData.LevelMetaData>();
            foreach(ReceiveLevelMetaData d in result.levels)
            {
                newLevels.Add(d.GetLevelMetaData());
            }
            levels = newLevels.ToArray();
        }
    }
}
