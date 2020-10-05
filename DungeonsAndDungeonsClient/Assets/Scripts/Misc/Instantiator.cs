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

        UnityEngine.Object[] o = Resources.LoadAll("pref/"+dir);
        GameObject[] result = new GameObject[o.Length];
        for (int i = 0; i < result.Length; i++)
        {
            Debug.Log(o[i]);
            result[i] = Instantiate(o[i]) as GameObject;
            //Evtl rework
            SceneManager.MoveGameObjectToScene(result[i], SceneManager.GetSceneByName(dir));
        }
        return result;
    }
}
