using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class FileManager : MonoBehaviour
{
    void Start()
    {
        UnityEngine.Debug.Log(Application.persistentDataPath);
    }
    void createFolder(string path)
    {
        string filePath = Application.persistentDataPath+path;
        try
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

        }
        catch (IOException ex)
        {
            UnityEngine.Debug.Log(ex.Message);
        }
    }
}
