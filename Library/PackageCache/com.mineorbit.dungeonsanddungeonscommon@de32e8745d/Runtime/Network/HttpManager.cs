using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Google.Protobuf;
using NetLevel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Net;
using CompressionLevel = System.IO.Compression.CompressionLevel;


namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class HttpManager : MonoBehaviour
    {
        public Option levelServerURL;

        public string baseURL;
        public static HttpManager instance;

        public FileStructureProfile compressedLevelFiles;
        public FileStructureProfile levelFolders;

        private string token;

        class TokenData
        {
            public string access_token;
            string token_type;
        }

        private void Start()
        {
            if (instance != null)
                Destroy(this);
            instance = this;
            baseURL = (string) levelServerURL.Value;
            Login("Max", "Test123");
        }

        private IEnumerator LoginRoutine(string username, string password)
        {
            var url = baseURL + $":8000/auth/token";
            var form = new WWWForm();
            form.AddField("grant_type", "");
            form.AddField("username", username);
            form.AddField("password", password);
            form.AddField("scope", "");
            form.AddField("client_id", "");
            form.AddField("client_secret", "");
            using (var www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                    GameConsole.Log(www.error);
                else
                {
                    GameConsole.Log(www.downloadHandler.text);
                    TokenData t = JsonUtility.FromJson<TokenData>(www.downloadHandler.text);
                    token = t.access_token;
                }
            }
        }

        public void Login(string username, string password)
        {
            StartCoroutine(LoginRoutine(username, password));
        }




        private IEnumerator UploadLevel(NetLevel.LevelMetaData levelToUpload, string path, UnityAction<string> action)
        {
            var url = baseURL +
                      $":8000/level/?proto_resp=true&name={levelToUpload.FullName}&description={levelToUpload.Description}";




            var form = new WWWForm();
            var fileByte = File.ReadAllBytes(path);
            form.AddBinaryData("levelFiles", fileByte, levelToUpload.UniqueLevelId + ".zip", "application / zip");

            action.Invoke("Uploading Level");
            using (var www = UnityWebRequest.Post(url, form))
            {
                www.SetRequestHeader("Authorization", $"Bearer {token}");

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                    action.Invoke(www.error);
                else
                    action.Invoke(www.downloadHandler.text);
            }
        }

        public LevelMetaData[] levelMetaDatas;

        private IEnumerator FetchLevelList(UnityEvent<string> reportAction, UnityEvent listUpdatedEvent)
        {
            var uri = baseURL + ":8000/level/all?proto_resp=true";


            GameConsole.Log("Fetching level list from " + uri);

            reportAction.Invoke("Loading Level List");
            using (var www = UnityWebRequest.Get(uri))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                    reportAction.Invoke(www.error);
                else
                {
                    string data = www.downloadHandler.text;
                    GameConsole.Log("Received " + data);
                    LevelMetaDataList list = LevelMetaDataList.Parser.ParseFrom(ByteString.CopyFromUtf8(data));
                    levelMetaDatas = list.Levels.ToArray();
                    GameConsole.Log("Got list: " + levelMetaDatas.Length);
                    foreach (var x in levelMetaDatas)
                    {
                        GameConsole.Log("MetaData: " + x);
                    }

                    listUpdatedEvent.Invoke();
                }



            }
        }



        private string AssembleZip(string resultPath, string targetPath)
        {
            //AlertScreen.alert.Open("Removing old File");
            File.Delete(resultPath);
            //AlertScreen.alert.Open("Compressing Level");

            try
            {
                Debug.Log($"Creating archive from {targetPath} to {resultPath}");
                ZipFile.CreateFromDirectory(targetPath, resultPath,
                    CompressionLevel.Optimal, false);

            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }

            return resultPath;
        }

        private string DisassembleZip(string targetPath, string resultPath)
        {
            Debug.Log($"Opening archive from {targetPath} to {resultPath}");
            try
            {
                ZipFile.ExtractToDirectory(targetPath, resultPath);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return resultPath;
        }



        public static void StartUpload(NetLevel.LevelMetaData levelToUpload)
        {
            
            count++;
            var path = instance.AssembleZip(
                instance.compressedLevelFiles.GetPath() + "" + count + ".zip",
                LevelDataManager.GetLevelPath(levelToUpload));
            instance.StartCoroutine(instance.UploadLevel(levelToUpload,
                instance.compressedLevelFiles.GetPath() + "" + count + ".zip",
                (x) => { Debug.Log("Test " + x); }));
        }

        public static void FetchLevelList(UnityEvent listUpdatedEvent)
        {
            Debug.Log("Fetching level list");
            UnityEvent<string> a = new UnityEvent<string>();
            instance.StartCoroutine(instance.FetchLevelList(a, listUpdatedEvent));
        }


        private IEnumerator DownloadLevelRoute(LevelMetaData toDownload)
        {
            var uri = baseURL + $":8000/level/download?proto_resp=false&ulid={toDownload.UniqueLevelId}";



            using (var www = UnityWebRequest.Get(uri))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {

                }
                else
                {
                    string savePath = instance.compressedLevelFiles.GetPath() + $"{toDownload.LocalLevelId}.zip";
                    string levelPath = instance.levelFolders.GetPath() + $"{toDownload.LocalLevelId}";


                    Debug.Log("Saving Data to " + savePath);
                    File.WriteAllText(savePath, www.downloadHandler.text);
                    MainCaller.Do(() => { instance.DisassembleZip(savePath, levelPath); });
                }



            }
        }

        private static int count;
        public static void DownloadLevel(LevelMetaData metaData)
        {
            count++;
            WebClient client = new WebClient();
            string savePath = instance.compressedLevelFiles.GetPath() + $"{count}.zip";
            string levelPath = LevelDataManager.SetupNewLevelFolder(metaData);
            var uri = instance.baseURL + $":8000/level/download?proto_resp=false&ulid={metaData.UniqueLevelId}";
            LevelDataManager.SaveLevelMetaData(metaData, levelPath+"/MetaData.json");
            client.DownloadFile(uri, savePath);
            instance.DisassembleZip(savePath, levelPath);
            
        }
    }
}