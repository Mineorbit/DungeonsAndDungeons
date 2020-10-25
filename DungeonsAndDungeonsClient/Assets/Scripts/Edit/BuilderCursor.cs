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

    static bool placementLegal = true;

    public void Start()
    {
        Debug.Log("Setting up Cursor");
        if (builderCursor != null) Destroy(this);
        builderCursor = this;

        cursorMesh = GetComponent<MeshFilter>();

        degree = 1f;
    }
    public static void Set(Vector3 target,Vector3 normal)
    {
        Vector3 position = new Vector3(Mathf.Round(target.x + normal.x), Mathf.Round(target.y + normal.y), Mathf.Round(target.z + normal.z));
        normalVec = normal;

        UpdateCursor(position);
    }

    public static void Set(LevelObjectData objectType)
    {
        currentSelection = objectType;
        UpdateRotation();
        UpdateMesh();
    }
    static void UpdateRotation()
    {
        builderCursor.transform.rotation = Quaternion.Euler(currentSelection.rotation);
    }
    void Update()
    {
        ComputeCursorPosition();
        placementLegal = UpdateLegal();
    }

    bool UpdateLegal()
    {

        int layerMask = 1 << 2;
        layerMask = ~layerMask;
        RaycastHit hit;
        float dist = 0.5f;
        if (Physics.Raycast(BuilderCursor.builderCursor.transform.position - 0.5f*dist* Vector3.forward, Vector3.forward, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(BuilderCursor.builderCursor.transform.position + 0.5f * dist * Vector3.forward, -Vector3.forward, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(BuilderCursor.builderCursor.transform.position - 0.5f * dist * Vector3.up, Vector3.up, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(BuilderCursor.builderCursor.transform.position + 0.5f * dist * Vector3.up, -Vector3.up, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(BuilderCursor.builderCursor.transform.position - 0.5f * dist * Vector3.right, Vector3.right, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(BuilderCursor.builderCursor.transform.position + 0.5f * dist * Vector3.right, -Vector3.right, out hit, dist, layerMask)) return false;
        return true;
    }

    void ComputeCursorPosition()
    {
        int layerMask = 1 << 2;
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
        builderCursor.transform.position = position;
    }
    static void UpdateMesh()
    {
        Debug.Log("Updating Mesh");
        Mesh targetMesh = null;
        if(currentSelection!=null)
        {
            targetMesh = currentSelection.GetMesh();
            Debug.Log("SM:"+targetMesh.subMeshCount);
            targetMesh.SetTriangles(targetMesh.triangles, 0);
            Debug.Log("SMN:" + targetMesh.subMeshCount);
            builderCursor.transform.localScale = currentSelection.Scale;
        }
        if (targetMesh != null)
        { 
            cursorMesh.mesh = targetMesh;
        }
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
    public static (Vector3 pos, Vector3 norm, LevelObjectData levelObjectData, bool legal) Get()
    {
        return (builderCursor.transform.position,normalVec, currentSelection, placementLegal);
    }
    public void OnDisable()
    {
        builderCursor = null;
    }
}
