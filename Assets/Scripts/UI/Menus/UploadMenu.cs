using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;
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
    
    public Option levelServerURL;

    public string baseURL;

    public FileStructureProfile levels;
    public FileStructureProfile compressedLevels;
    public override void Awake()
    {
        base.Awake();
        index = 5;
        var Interface = transform.Find("Interface");
        nameText = Interface.Find("LevelName").GetComponent<TextMeshProUGUI>();
        uploadButton = Interface.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(StartUpload);
        baseURL = (string) levelServerURL.Value;
    }


    class Zip
    {
        // https://www.codeproject.com/Tips/319438/How-to-Compress-Decompress-directories
        
        public delegate void ProgressDelegate(string sMessage);
         static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
    {
      //Compress file name
      char[] chars = sRelativePath.ToCharArray();
      zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
      foreach (char c in chars)
        zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));

      //Compress file content
      byte[] bytes = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
      zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
      zipStream.Write(bytes, 0, bytes.Length);
    }

    static bool DecompressFile(string sDir, GZipStream zipStream, ProgressDelegate progress)
    {
      //Decompress file name
      byte[] bytes = new byte[sizeof(int)];
      int Readed = zipStream.Read(bytes, 0, sizeof(int));
      if (Readed < sizeof(int))
        return false;

      int iNameLen = BitConverter.ToInt32(bytes, 0);
      bytes = new byte[sizeof(char)];
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < iNameLen; i++)
      {
        zipStream.Read(bytes, 0, sizeof(char));
        char c = BitConverter.ToChar(bytes, 0);
        sb.Append(c);
      }
      string sFileName = sb.ToString();
      if (progress != null)
        progress(sFileName);

      //Decompress file content
      bytes = new byte[sizeof(int)];
      zipStream.Read(bytes, 0, sizeof(int));
      int iFileLen = BitConverter.ToInt32(bytes, 0);

      bytes = new byte[iFileLen];
      zipStream.Read(bytes, 0, bytes.Length);

      string sFilePath = Path.Combine(sDir, sFileName);
      string sFinalDir = Path.GetDirectoryName(sFilePath);
      if (!Directory.Exists(sFinalDir))
        Directory.CreateDirectory(sFinalDir);

      using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
        outFile.Write(bytes, 0, iFileLen);

      return true;
    }

    public static void CompressDirectory(string sInDir, string sOutFile, ProgressDelegate progress)
    {
      string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
      int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

      using (FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
      using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
        foreach (string sFilePath in sFiles)
        {
          string sRelativePath = sFilePath.Substring(iDirLen);
          if (progress != null)
            progress(sRelativePath);
          CompressFile(sInDir, sRelativePath, str);
        }
    }

    public static void DecompressToDirectory(string sCompressedFile, string sDir, ProgressDelegate progress)
    {
      using (FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
      using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
        while (DecompressFile(sDir, zipStream, progress));
    }

    }
    
    

    private IEnumerator Upload(string path)
    {
        var url = baseURL+"/upload";
        
        
        var fileByte = File.ReadAllBytes(path);
        var form = new WWWForm();
        form.AddField("name", levelToUpload.FullName);
        form.AddBinaryData("level", fileByte, levelToUpload.uniqueLevelId + ".zip", "application / zip");

        AlertScreen.alert.Open("Uploading Level");
        using (var www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                AlertScreen.alert.Open(www.error);
            else
                AlertScreen.alert.Open(www.downloadHandler.text);
        }
    }


    public void StartUpload()
    {
        var path = AssembleZip();
        StartCoroutine(Upload(path));
    }

    private string AssembleZip()
    {
        string levelPath = levels.GetPath() + levelToUpload.localLevelId;
        string resultPath = compressedLevels.GetPath()+levelToUpload.localLevelId + ".zip";
        AlertScreen.alert.Open("Removing old File");
        File.Delete(resultPath);
        AlertScreen.alert.Open("Compressing Level");
        
        Zip.CompressDirectory(levelPath,resultPath, (x) => { 
            AlertScreen.alert.Open("Compressing "+x);});
        return resultPath;
    }

}