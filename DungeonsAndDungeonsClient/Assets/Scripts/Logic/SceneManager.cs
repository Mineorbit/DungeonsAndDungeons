using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static int numberOfScenes = 3;  
   public static int currentScene = 0;
   public static void load(int loadScene)
    {
        if (loadScene >= numberOfScenes)
        { UnityEngine.SceneManagement.SceneManager.LoadScene(1, LoadSceneMode.Additive); }else
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadScene, LoadSceneMode.Additive);
    }
    public static void unload()
    {

    }
}
