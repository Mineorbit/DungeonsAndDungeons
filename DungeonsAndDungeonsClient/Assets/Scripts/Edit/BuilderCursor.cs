using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCursor : MonoBehaviour
{
    static float degree;
    static BuilderCursor builderCursor;

    static Vector3 normalVec;

    public void Awake()
    {
        if (builderCursor != null) Destroy(this);
        builderCursor = this;
        degree = 1f;
    }
    public static void Set(Vector3 target,Vector3 normal)
    {
        Vector3 position = new Vector3(Mathf.Round(target.x/degree), Mathf.Round(target.y / degree), Mathf.Round(target.z / degree));
        position = degree * position + normal * degree;
        builderCursor.transform.position = position;
        normalVec = normal;
    }
    public static (Vector3 pos, Vector3 norm) Get()
    {
        return (builderCursor.transform.position,normalVec);
    }
}
