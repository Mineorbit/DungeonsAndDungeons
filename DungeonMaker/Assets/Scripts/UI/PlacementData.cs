using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementData : MonoBehaviour
{
    public Vector3 Displacement;
    public void Start()
    {
        transform.position += Displacement;
    }
}
