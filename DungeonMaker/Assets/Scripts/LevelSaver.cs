using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelSaver 
 {
	public void save(string path, LevelData data)
	{
		if(!File.Exists(Application.persistentDataPath + "/map/" ))
		{
			System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Application.persistentDataPath,"map"));
		}
		BinaryFormatter formatter = new BinaryFormatter();
		string targetpath = Application.persistentDataPath+"/"+path+".lev";
		FileStream stream = new FileStream(targetpath,FileMode.Create);
		formatter.Serialize(stream,data);
		stream.Close();
		Debug.Log("Saved to: "+targetpath);
	}
}
