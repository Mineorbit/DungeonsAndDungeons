using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class LevelLoader
{
	public LevelData load(string path)
	{
		string targetpath = Application.persistentDataPath+path;

		if(File.Exists(targetpath))
		{
			BinaryFormatter formatter = new  BinaryFormatter();
			FileStream stream = new FileStream(targetpath,FileMode.Open);
			LevelData d = (LevelData) formatter.Deserialize(stream);
			stream.Close();
			foreach(LevelObjectData dd in d.levelObjects)
			return d;
		}else
		{
		Debug.LogError("Level existiert nicht: "+targetpath);
		return null;
		}
		return null;
	}
}
