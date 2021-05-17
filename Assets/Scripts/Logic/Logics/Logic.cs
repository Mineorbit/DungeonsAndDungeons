using System.Collections.Generic;
using UnityEngine;

public class Logic
{
    public GameObject[] created;
    public bool running;
    public int sceneIndex;

    public GameObject[] FetchAllinScene()
    {
        var objs = new List<GameObject>();
        foreach (var obj in Resources.FindObjectsOfTypeAll<GameObject>())
            if (obj.scene.buildIndex == sceneIndex)
                objs.Add(obj);
        if (objs.Count == 0) return new GameObject[0];
        return objs.ToArray();
    }

    public void SpawnAll()
    {
        created = FetchAllinScene();
        foreach (var g in created) g.SetActive(true);
    }

    public void DespawnAll()
    {
        created = FetchAllinScene();
        foreach (var g in created) g.SetActive(false);
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