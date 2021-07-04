using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class FileManager : MonoBehaviour
    {
        public static TaskCompletionSource<bool> foldersCreated = new TaskCompletionSource<bool>();
        public FileStructureProfile root;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            foreach (var s in List(root)) createFolder(s);
            foldersCreated.SetResult(true);
        }


        private string[] List(FileStructureProfile profile)
        {
            if (profile == null) return new string[0];
            var path = "/" + profile.fileStructureProfileName;
            var paths = new List<string>();
            paths.Add(path);
            foreach (var fS in profile.subStructures)
            {
                var subList = List(fS);
                var resultList = new string[subList.Length];
                var i = 0;
                foreach (var subpath in subList)
                {
                    resultList[i] = path + subpath;
                    i++;
                }

                paths.AddRange(resultList);
            }

            return paths.ToArray();
        }

        public static void deleteFolder(string path)
        {
            var filePath = Application.persistentDataPath + path;
            if (Directory.Exists(filePath)) Directory.Delete(filePath, true);
        }

        public static void createFolder(string path, bool persistent = true)
        {
            var filePath = (persistent ? Application.persistentDataPath : "") + path;

            GameConsole.Log("Erstelle Ordner: " + filePath);
            try
            {
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                else GameConsole.Log("Ordner gibt es schon");
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }
}