using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InstantiationTarget", order = 1)]
public class InstantionTarget : ScriptableObject
{
    public UnityEngine.Object asset;
    
    public GameObject Create(Vector3 location)
    {
        return Instantiator.Instantiate(asset,location);
    }
}
