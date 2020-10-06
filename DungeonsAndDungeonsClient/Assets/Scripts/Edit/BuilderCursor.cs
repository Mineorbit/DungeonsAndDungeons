using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCursor : MonoBehaviour
{
    static float degree;
    static BuilderCursor builderCursor;
    public void Awake()
    {
        if (builderCursor != null) Destroy(this);
        builderCursor = this;
        degree = 1;
    }
    public static void Set(Vector3 loc,Vector3 normal)
    {
        Vector3 target = loc + normal*degree/2;
        Vector3 position = new Vector3(Mathf.Round(target.x), Mathf.Round(target.y), Mathf.Round(target.z));

        builderCursor.transform.position = position;
    }
    public static Vector3 Get()
    {
        return builderCursor.transform.position;
    }
}
