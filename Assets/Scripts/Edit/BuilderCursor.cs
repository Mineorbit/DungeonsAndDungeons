using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

using com.mineorbit.dungeonsanddungeonscommon;

public class BuilderCursor : MonoBehaviour
{
    float maxDistance = 200;
    static float degree;
    public static BuilderCursor builderCursor;

    static LevelObjectData currentSelection;


    static Vector3 normalVec;

    static MeshFilter cursorMesh;
    static MeshRenderer cursorRender;


    static GameObject hitObject;
    static Transform cursorModel;

    static bool placementLegal = false;
    static Vector3 offset;

    public Material legalMat;
    public Material illlegalMat;

    public int direction;



    public void OnEnable()
    {
        if (builderCursor != null) Destroy(this);
        builderCursor = this;
        cursorModel = transform.Find("model");
        cursorMesh = cursorModel.GetComponent<MeshFilter>();
        cursorRender = cursorModel.GetComponent<MeshRenderer>();
        
        degree = 1f;
    }
    public void SetLocation(Vector3 target,Vector3 normal)
    {
        float g = 1;
        Vector3 position = LevelManager.GetGridPosition(target+normal, currentSelection);
        normalVec = normal;
        if(currentSelection!= null)
        {
            //offset = currentSelection.cursorOffset;
            offset = new Vector3(0,0,0);
        }
        UpdateCursor(position,offset);
    }

    public static void Set(LevelObjectData objectType)
    {
        currentSelection = objectType;
        UpdateMesh();
    }


    public void RotateLeft()
    {
        direction = (direction - 1) % 4;
    }

    public void RotateRight()
    {
        direction = (direction + 1) % 4;
    }
    void Update()
    {
        ComputeCursorPosition();
        UpdateMaterial();
    }
    void UpdateMaterial()
    {
        bool newLegal = UpdateLegal();
        if(newLegal!=placementLegal)
        {
            placementLegal = newLegal;
            if(placementLegal)
            {
                cursorRender.material = builderCursor.legalMat;
            }else
            {
                cursorRender.material = builderCursor.illlegalMat;
            }
        }
    }

    bool UpdateLegal()
    {

        int layerMask = 1 << 2;
        layerMask = ~layerMask;
        RaycastHit hit;
        float dist = 0.5f;
        if (Physics.Raycast(cursorModel.transform.position - 0.5f*dist* Vector3.forward, Vector3.forward, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position + 0.5f * dist * Vector3.forward, -Vector3.forward, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position - 0.5f * dist * Vector3.up, Vector3.up, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position + 0.5f * dist * Vector3.up, -Vector3.up, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position - 0.5f * dist * Vector3.right, Vector3.right, out hit, dist, layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position + 0.5f * dist * Vector3.right, -Vector3.right, out hit, dist, layerMask)) return false;
        return true;
    }

    void ComputeCursorPosition()
    {
        int layerMask = 1 << 2;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 targetLocation;

        Vector3 start = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        if (Physics.Raycast(start, forward, out hit, Mathf.Infinity, layerMask))
        {
            targetLocation = start + forward * hit.distance;
            hitObject = hit.transform.gameObject;
        }
        else
        {
            targetLocation = start + forward * maxDistance;
        }
        SetLocation(targetLocation, hit.normal);
    }
    void UpdateCursor(Vector3 position,Vector3 offset)
    {
        builderCursor.transform.position = position;

        Vector3 rot = new Vector3(0,0,0);
        Vector3 scal = new Vector3(1,1,1);
        if (currentSelection != null)
        {
            //rot = currentSelection.cursorRotation;
            //scal = currentSelection.cursorScale;
        }
        builderCursor.transform.rotation = Quaternion.Euler(rot);
        builderCursor.transform.localScale = scal;
        cursorModel.transform.position = position + cursorModel.transform.TransformVector(offset);
        builderCursor.transform.RotateAround(builderCursor.transform.position, Vector3.up, builderCursor.direction * 90f);
    }
    static void UpdateMesh()
    {
        Mesh targetMesh = null;
        if(currentSelection!=null)
        {
           // targetMesh = currentSelection.GetMesh();
           // targetMesh.SetTriangles(targetMesh.triangles, 0);
        }
        if (targetMesh != null)
        { 
            cursorMesh.mesh = targetMesh;
        }
    }

    public LevelObject GetLevelObjectAt()
    {
        LevelObject o = hitObject.GetComponent<LevelObject>(); 
        if(o != null)
        {
            return o;
        }
        return null;
    }

    public GameObject GetGameObjectAt()
    {
        GameObject o = hitObject;
        if (o != null)
        {
            return o;
        }
        return null;
    }

    public (Vector3 pos, Quaternion rot, Vector3 norm, LevelObjectData levelObjectData, bool legal) Get()
    {
        return (cursorModel.transform.position, builderCursor.transform.rotation, normalVec, currentSelection, placementLegal);
    }
    public void OnDisable()
    {
        builderCursor = null;
    }
}
