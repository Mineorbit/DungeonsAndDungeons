using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic
{
    public int sceneIndex;
    public GameObject[] created;
    public bool running;

    public GameObject[] FetchAllinScene()
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.scene.buildIndex == sceneIndex)
            {
                objs.Add(obj);
            }
        }
        if(objs.Count==0)
        {
            return new GameObject[0];
        }
        return objs.ToArray();
    }
    public void RemoveAll()
    {
        if(created == null)
        {
            created = FetchAllinScene();
        }else if(created.Length==0)
        {
            created = FetchAllinScene();
        }
        if(created!=null)
        if(created.Length>0)
        foreach (GameObject g in created)
        {
            Instantiator.Remove(g);
        }
    }
    public void SpawnAll()
    {
        created = FetchAllinScene();
        foreach (GameObject g in created)
        {
            g.SetActive(true);
        }
    }
    public void DespawnAll()
    {

        created = FetchAllinScene();
        foreach (GameObject g in created)
        {
            g.SetActive(false);
        }
    }
    public virtual void Init()
    {

    }
    public virtual void Start()
    {
        running = true;
    }
    public virtual void Stop()
    {
        running = false;
    }
    public virtual void DeInit()
    {
         
    }
}
