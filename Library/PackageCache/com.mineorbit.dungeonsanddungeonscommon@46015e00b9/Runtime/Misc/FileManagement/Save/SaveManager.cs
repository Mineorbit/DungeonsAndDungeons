using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class SaveManager
    {
        public enum StorageType
        {
            BIN,
            XML,
            JSON
        }

        private readonly StorageType storageType;

        public SaveManager(StorageType t)
        {
            storageType = t;
        }

        public T Load<T>(string path)
        {
            var result = default(T);
            if (!File.Exists(path)) return result;

            if (storageType == StorageType.JSON)
            {
                var reader = new StreamReader(path);
                var data = reader.ReadToEnd();
                reader.Close();

                result = JsonUtility.FromJson<T>(data);
            }
            else if (storageType == StorageType.BIN)
            {
                var fs = new FileStream(path, FileMode.Open);
                try
                {
                    var formatter = new BinaryFormatter();

                    // Deserialize the hashtable from the file and
                    // assign the reference to the local variable.
                    result = (T) formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Debug.Log("Failed to deserialize. Reason: " + e.Message);
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

        public bool Save(object o, string path, bool persistent = true)
        {
            var filePath = (persistent ? Application.persistentDataPath : "") + path;
            if (storageType == StorageType.JSON)
            {
                var writer = new StreamWriter(filePath);
                var content = JsonUtility.ToJson(o);
                writer.WriteLine(content);
                writer.Flush();
                writer.Close();
            }
            else if (storageType == StorageType.BIN)
            {
                var fs = new FileStream(filePath, FileMode.Create);

                var formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, o);
                }
                catch (SerializationException e)
                {
                    Debug.Log("Failed to serialize. Reason: " + e.Message);
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
}