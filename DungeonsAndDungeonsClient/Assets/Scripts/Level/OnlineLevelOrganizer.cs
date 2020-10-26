using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    LevelData.LevelMetaData[] levels;
    public void Start()
    {
        FetchPopularLevels();
    }
    public void FetchPopularLevels()
    {
        Debug.Log("Fetching List of online Levels");
        StartCoroutine("FetchList");
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
