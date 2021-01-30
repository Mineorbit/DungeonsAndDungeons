using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelObjectData", order = 1)]
public class LevelObjectData : InstantionTarget
{
    public string FullName;
    public float granularity;
    public int ID;
    public Mesh objectMesh;
    public Vector3 cursorCenterOfRotation;
    public Vector3 cursorOffset;
    public Vector3 cursorScale;
    public Vector3 cursorRotation;

    public Vector3 place_scale;
    public Vector3 place_rotation;
    public Vector3 place_offset;
    public virtual GameObject Create(Vector3 location, Transform parent)
    { 
        GameObject g = base.Create(location,parent);
        g.GetComponent<LevelObject>().ObjectDataID = this.ID;
        g.GetComponent<LevelObject>().chunk = parent.GetComponent<Chunk>();
        g.transform.localScale = place_scale;
        //g.transform.position = location + g.transform.TransformVector(place_offset);
        g.GetComponent<LevelObject>().OnCreate();
        return g;
    }
    public virtual GameObject Create(Vector3 location, Quaternion rotation, Transform parent)
    {
        GameObject g = base.Create(location, parent);
        g.GetComponent<LevelObject>().ObjectDataID = this.ID;
        g.GetComponent<LevelObject>().chunk = parent.GetComponent<Chunk>();
        g.transform.localScale = place_scale;
        Debug.Log(rotation.eulerAngles);
        g.transform.position = location+rotation*place_offset;
        Quaternion rot =  rotation * Quaternion.Euler(place_rotation);
        g.transform.rotation = rot;
        g.GetComponent<LevelObject>().OnCreate();
        return g;
    }
    public Mesh GetMesh()
    {
        if(objectMesh==null)
        {
            GameObject g = base.Create(new Vector3(0,0,0));
            g.SetActive(false);
            objectMesh = g.GetComponentInChildren<MeshFilter>().mesh;
            Destroy(g);
        }
        return objectMesh;
    }
}
