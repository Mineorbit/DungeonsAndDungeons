using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
public class LevelSaver 
 {
	void save(string path, LevelData data)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string targetpath = Application.persistentDataPath+path+".lev"
		FileStream stream = new FileStream();
	}
}
