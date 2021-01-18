using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public List<Chunk> chunks;
    float dist = 48;
    public Transform target;

    bool allActive = true;


    void FixedUpdate()
    {
        FindTarget();
        if (target!=null)
        {
        allActive = false;
        Vector3 targetPos = target.position;
        targetPos.y = 0;
        chunks = Level.GetChunks();
        foreach (Chunk c in chunks)
        {
                if(c!=null)
                {
                    Vector3 chunkPos = c.transform.position + new Vector3(16, 0, 16);
                    chunkPos.y = 0;
                    c.gameObject.SetActive((chunkPos - targetPos).magnitude <= dist);
                }
        }
        }else if(!allActive)
        {
            allActive = true;
            ActivateAllChunks();
        }
    }

    void FindTarget()
    {
        if(GameManager.GetState() == GameManager.State.Edit)
        {
            GameObject p = GameObject.Find("Builder");
            if(p!=null)
            target = p.transform;
        }
        if (GameManager.GetState() == GameManager.State.Test)
        {
            target = null;
        }
    }

    public static void ActivateAllChunks()
    {
        foreach (Chunk c in Level.GetChunks())
        {
            if(c!=null)
                c.gameObject.SetActive(true);
        }
    }

}
