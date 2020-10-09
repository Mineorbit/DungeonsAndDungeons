using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCursor : MonoBehaviour
{
    float maxDistance = 200;
    static float degree;
    static BuilderCursor builderCursor;

    static LevelObjectData currentSelection;


    static Vector3 normalVec;

    static MeshFilter cursorMesh;

    static GameObject hitObject;

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

    public static void Set(LevelObjectData objectType)
    {
        currentSelection = objectType;
        UpdateMesh();
    }
    void Update()
    {
        ComputeCursorPosition();
    }
    void ComputeCursorPosition()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 targetLocation;
        if (Physics.Raycast(BuilderController.builderPosition, BuilderController.builderForward, out hit, Mathf.Infinity, layerMask))
        {
            targetLocation = BuilderController.builderPosition + BuilderController.builderForward * hit.distance;
            hitObject = hit.transform.gameObject;
        }
        else
        {
            targetLocation = BuilderController.builderPosition + BuilderController.builderForward * maxDistance;
        }
        BuilderCursor.Set(targetLocation, hit.normal);
    }
    static void UpdateCursor(Vector3 position)
    {
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
    public static LevelObject GetObjectAt()
    {
        LevelObject o = hitObject.GetComponent<LevelObject>(); 
        if(o != null)
        {
            return o;
        }
        return null;
    }
    public static (Vector3 pos, Vector3 norm, LevelObjectData levelObjectData) Get()
    {
        return (builderCursor.transform.position,normalVec, currentSelection);
    }
}
