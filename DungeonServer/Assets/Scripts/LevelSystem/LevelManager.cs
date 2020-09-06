using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static Level currentLevel;

    void Start()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }
    public void prepareLevel()
    {
        /*
        if (!checkLevelAvailable(levelId)) { downloadLevel(levelId); return; }
        loadLevel();
        */
    }

    void Update()
    {
        
    }
}
