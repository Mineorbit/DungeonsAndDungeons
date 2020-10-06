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
        float x = Mathf.Round(loc.x / degree)  * degree;
        float y = Mathf.Round(loc.y / degree)  * degree;
        float z = Mathf.Round(loc.z / degree) * degree;
        Vector3 pos = new Vector3(x,y,z);

        //builderCursor.transform.position = pos + normal*degree/2;
        builderCursor.transform.position = loc + normal*degree/2;
    }
    public static Vector3 Get()
    {
        return builderCursor.transform.position;
    }
}
