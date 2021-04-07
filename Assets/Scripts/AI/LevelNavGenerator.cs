using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LevelNavGenerator : MonoBehaviour
{
    public static NavMeshSurface navMeshSurface;
    void Start()
    {
        if(navMeshSurface == null) navMeshSurface = GetComponent<NavMeshSurface>();

    }

    public static void UpdateNavMesh()
    {
    Debug.Log("Building");
    Enemy[] enemies = Level.GetAllEnemies();
    foreach(Enemy e in enemies)
    {
            if(e != null)
            e.gameObject.SetActive(false);
    }

    navMeshSurface.BuildNavMesh();

    foreach (Enemy e in enemies)
    {
            if (e != null)
                e.gameObject.SetActive(true);
    }
    }
}
