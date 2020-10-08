using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InstantiationTarget", order = 1)]
public class InstantionTarget : ScriptableObject
{
    public UnityEngine.Object asset;
    
    public virtual GameObject Create(Vector3 location, Transform parent)
    {
        GameObject g = Instantiator.Instantiate(asset, location);
        g.transform.SetParent(parent);
        return g;
    }
    public virtual GameObject Create(Vector3 location)
    {
        GameObject g = Instantiator.Instantiate(asset, location);
        return g;
    }
}
