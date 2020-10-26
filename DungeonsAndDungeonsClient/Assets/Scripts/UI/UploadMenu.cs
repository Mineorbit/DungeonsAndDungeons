using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO.Compression;

public class UploadMenu : MenuPage
{
    public static LevelData.LevelMetaData levelToUpload;


    TMPro.TextMeshProUGUI nameText;
    Button uploadButton;

    public override void Awake()
    {
        base.Awake();
        index = 5;
        Transform Interface = transform.Find("Interface");
        nameText = Interface.Find("LevelName").GetComponent<TMPro.TextMeshProUGUI>();
        uploadButton = Interface.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(StartUpload);
    }
    public override void Start()
    {
        base.Start();

    }

    IEnumerator Upload(string path)
    {
        string url = "http://josch557.xyz:13337/upl";

        byte[] fileByte = File.ReadAllBytes(path);
        WWWForm form = new WWWForm();
        form.AddField("name", levelToUpload.name);
        form.AddBinaryData("level", fileByte, levelToUpload.ulid + ".zip", "application / zip");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                AlertScreen.alert.Open(www.error);
            }
            else
            {
                AlertScreen.alert.Open(www.downloadHandler.text);
            }
        }
    }

    public override void Open()
    {
        nameText.text = "Name: " + levelToUpload.name+ " \nLocalID: "+levelToUpload.ullid;
        base.Open();
    }

    public void StartUpload()
    {
        string path = AssembleZip();
        StartCoroutine(Upload(path));
    }
    string AssembleZip()
    {
        string levelPath = Application.persistentDataPath + "/gameData/levels/" + levelToUpload.ullid;
        string resultPath = Application.persistentDataPath + "/gameData/c_levels/" + levelToUpload.ullid + ".zip";
        File.Delete(resultPath);
        ZipFile.CreateFromDirectory(levelPath, resultPath);
        return resultPath;
    }
}
