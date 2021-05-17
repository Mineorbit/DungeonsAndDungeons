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
        var o = Resources.LoadAll("pref/" + dir);
        var result = new GameObject[o.Length];
        for (var i = 0; i < result.Length; i++)
        {
            //Instantiate
            result[i] = Instantiate(o[i]) as GameObject;

            // remove " (Clone)"
            //Enumeration Code wrong later need an asset(ScriptableObject) for instantiation list
            var name = result[i].name;
            var newname = name.Substring(0, name.Length - 7);
            result[i].name = newname;
            //Move to the associated scene

            if (SceneManager.GetSceneByName(dir) != null)
                SceneManager.MoveGameObjectToScene(result[i], SceneManager.GetSceneByName(dir));
        }

        return result;
    }

    public static GameObject Instantiate(Object prefab, Vector3 location)
    {
        var o = Instantiate(prefab) as GameObject;
        o.transform.position = location;
        return o;
    }

    public static void Remove(GameObject g)
    {
        Destroy(g);
    }

    public static GameObject[] Instantiate(Object prefab, Vector3[] locations)
    {
        var o = new GameObject[locations.Length];
        for (var i = 0; i < locations.Length; i++)
        {
            o[i] = Instantiate(prefab) as GameObject;
            o[i].transform.position = locations[i];
        }

        return o;
    }

    public static void Move(GameObject[] gameObject, string dir)
    {
        for (var i = 0; i < gameObject.Length; i++)
            SceneManager.MoveGameObjectToScene(gameObject[i], SceneManager.GetSceneByName(dir));
    }
}