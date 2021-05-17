using System.Collections;
using System.IO;
using com.mineorbit.dungeonsanddungeonscommon;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UploadMenu : MenuPage
{
    public static LevelMetaData levelToUpload;


    private TextMeshProUGUI nameText;
    private Button uploadButton;

    public override void Awake()
    {
        base.Awake();
        index = 5;
        var Interface = transform.Find("Interface");
        nameText = Interface.Find("LevelName").GetComponent<TextMeshProUGUI>();
        uploadButton = Interface.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(StartUpload);
    }

    public override void Start()
    {
        base.Start();
    }

    private IEnumerator Upload(string path)
    {
        var url = "http://josch557.xyz:13337/upl";

        var fileByte = File.ReadAllBytes(path);
        var form = new WWWForm();
        form.AddField("name", levelToUpload.FullName);
        form.AddBinaryData("level", fileByte, levelToUpload.uniqueLevelId + ".zip", "application / zip");

        using (var www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                AlertScreen.alert.Open(www.error);
            else
                AlertScreen.alert.Open(www.downloadHandler.text);
        }
    }

    public override void OnOpen()
    {
        nameText.text = "Name: " + levelToUpload.FullName + " \nLocalID: " + levelToUpload.localLevelId;
        Open();
    }

    public void StartUpload()
    {
        var path = AssembleZip();
        StartCoroutine(Upload(path));
    }

    private string AssembleZip()
    {
        /*
        string levelPath = Application.persistentDataPath + "/gameData/levels/" + levelToUpload.localLevelId;
        string resultPath = Application.persistentDataPath + "/gameData/c_levels/" + levelToUpload.localLevelId + ".zip";
        File.Delete(resultPath);
        ZipFile.CreateFromDirectory(levelPath, resultPath);
        return resultPath;
        */
        return "";
    }
}