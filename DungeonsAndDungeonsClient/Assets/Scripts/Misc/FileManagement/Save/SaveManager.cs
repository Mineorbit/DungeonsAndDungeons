using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
public class SaveManager
{
    public enum StorageType { BIN, XML, JSON };
    StorageType storageType;
    public SaveManager(StorageType t)
    {
        storageType = t;
    }
    public T Load<T>(string path)
    {
        if (!File.Exists(path)) return default(T);

        StreamReader reader = new StreamReader(path);
        string data = reader.ReadToEnd();
        T result = JsonUtility.FromJson<T>(data);
        return result;
    }
    public bool Save(object o,string path)
    {
        string filePath = Application.persistentDataPath + path;
        StreamWriter writer = new StreamWriter(filePath);
        string content = JsonUtility.ToJson(o);
        writer.WriteLine(content);
        writer.Flush();
        writer.Close();

        return true;
    }
}
