using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instantiator : MonoBehaviour
{
    public static Instantiator instantiator;
    public void Awake()
    {
        if (instantiator != null) Destroy(this);
        instantiator = this;
    }
    public static GameObject[] InstantiateAssets(string dir)
    {
        //Load Scene Prefab Directory
        UnityEngine.Object[] o = Resources.LoadAll("pref/"+dir);
        GameObject[] result = new GameObject[o.Length];
        for (int i = 0; i < result.Length; i++)
        {
            //Instantiate
            result[i] = Instantiate(o[i]) as GameObject;

            // remove " (Clone)"
            //Enumeration Code wrong later need an asset(ScriptableObject) for instantiation list
            string name = result[i].name;
            string newname = name.Substring(0, name.Length - 7);
            result[i].name = newname;
            //Move to the associated scene
            SceneManager.MoveGameObjectToScene(result[i], SceneManager.GetSceneByName(dir));
        }
        return result;
    }
    public static GameObject Instantiate(UnityEngine.Object prefab, Vector3 location)
    {
        GameObject o = Instantiate(prefab) as GameObject;
        o.transform.position = location;
        return o;
    }
    public static GameObject[] Instantiate(UnityEngine.Object prefab, Vector3[] locations)
    {
        GameObject[] o = new GameObject[locations.Length];
        for(int i = 0;i<locations.Length;i++)
        {
            o[i] = Instantiate(prefab) as GameObject;
            o[i].transform.position = locations[i];
        }
        return o;
    }
    
    public static void Move(GameObject[] gameObject, string dir)
    {
        for(int i = 0;i<gameObject.Length;i++)
            SceneManager.MoveGameObjectToScene(gameObject[i], SceneManager.GetSceneByName(dir));
    }
}
