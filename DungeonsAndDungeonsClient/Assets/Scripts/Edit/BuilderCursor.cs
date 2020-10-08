using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCursor : MonoBehaviour
{
    static float degree;
    static BuilderCursor builderCursor;

    public static LevelObjectData currentSelection;
    //Dummy
    public LevelObjectData curSel;


    static Vector3 normalVec;

    static MeshFilter cursorMesh;

    public void Awake()
    {
        if (builderCursor != null) Destroy(this);
        builderCursor = this;

        cursorMesh = GetComponent<MeshFilter>();

        degree = 1f;
    }
    public static void Set(Vector3 target,Vector3 normal)
    {
        Vector3 position = new Vector3(Mathf.Round(target.x/degree), Mathf.Round(target.y / degree), Mathf.Round(target.z / degree));
        position = degree * position + normal * degree;
        UpdateCursor(position);
        normalVec = normal;
    }

    static void UpdateCursor(Vector3 position)
    {
        //Dummy
        currentSelection = builderCursor.curSel;
        UpdateMesh();

        builderCursor.transform.position = position;
    }
    static void UpdateMesh()
    {
        Mesh targetMesh = null;
        if(currentSelection!=null)
        {
            targetMesh = currentSelection.GetMesh();
            builderCursor.transform.localScale = currentSelection.Scale;
        }
        if (targetMesh != null)
            cursorMesh.mesh = targetMesh;

    }
    public static (Vector3 pos, Vector3 norm, LevelObjectData levelObjectData) Get()
    {
        return (builderCursor.transform.position,normalVec, currentSelection);
    }
}
