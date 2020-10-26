using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
        T result = default(T);
        if (!File.Exists(path)) return result;

        if(storageType == StorageType.JSON)
        { 
        StreamReader reader = new StreamReader(path);
        string data = reader.ReadToEnd();
        reader.Close();
        result = JsonUtility.FromJson<T>(data);
        }
        else if(storageType == StorageType.BIN)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and
                // assign the reference to the local variable.
                result = (T) formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                UnityEngine.Debug.Log("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return result;
        }

        return result;
    }
    public bool Save(object o,string path)
    {
        if(storageType == StorageType.JSON)
        { 
        string filePath = Application.persistentDataPath + path;
        StreamWriter writer = new StreamWriter(filePath);
        string content = JsonUtility.ToJson(o);
        writer.WriteLine(content);
        writer.Flush();
        writer.Close();

        }
        else if(storageType == StorageType.BIN)
        {
            FileStream fs = new FileStream(Application.persistentDataPath + path, FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, o);
            }
            catch (SerializationException e)
            {
                UnityEngine.Debug.Log("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        return true;
    }
}
