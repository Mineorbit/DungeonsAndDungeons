using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelObjectData", order = 1)]
public class LevelObjectData : InstantionTarget
{
    public string FullName;
    public Vector3 Scale;
    public Vector3 rotation;
    public Vector3 offset;
    public int ID;
    Mesh objectMesh;
    public virtual GameObject Create(Vector3 location, Transform parent)
    { 
        GameObject g = base.Create(location,parent);
        g.GetComponent<LevelObject>().ObjectDataID = this.ID;
        g.GetComponent<LevelObject>().chunk = parent.GetComponent<Chunk>();
        g.transform.localScale = Scale;
        g.transform.position = location + offset;

        return g;
    }
    public Mesh GetMesh()
    {
        if(objectMesh==null)
        {
            GameObject g = base.Create(new Vector3(0,0,0));
            g.SetActive(false);
            objectMesh = g.GetComponent<MeshFilter>().mesh;
            Destroy(g);
        }
        return objectMesh;
    }
}
