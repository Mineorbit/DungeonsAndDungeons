using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public LevelData[] levels;


    void Awake()
    {
        if (levelManager != null) Destroy(this);
        levelManager = this;
    }
    public void New(LevelData.LevelMetaData levelMetaData)
    {

    }
    
}
