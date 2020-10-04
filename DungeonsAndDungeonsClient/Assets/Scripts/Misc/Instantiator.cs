using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public static Instantiator instantiator;
    public void Awake()
    {
        if (instantiator != null) Destroy(this);
        instantiator = this;
    }
    public static GameObject[] InstantiateAssets()
    {

        UnityEngine.Object[] o = Resources.LoadAll("pref/edit");
        GameObject[] result = new GameObject[o.Length];
        for (int i = 0; i < result.Length; i++)
        {
            Debug.Log(o[i]);
            result[i] = Instantiate(o[i]) as GameObject;
        }
        return result;
    }
}
