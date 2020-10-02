using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SceneLoadManager : MonoBehaviour
{
    private int numberOfScenes = 3;  
    public int currentScene = 0;
    private IEnumerator coroutine;
    public static SceneLoadManager instance;
    public void Awake()
    {
        if(instance!=null)
        {
            Destroy(this);
        }
        instance = this;
    }
    /*
    public void load(int loadScene)
    {
        if (loadScene >= numberOfScenes)
        { UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Additive); }else
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
    */
    public void load(int loadScene,UnityEvent finishEvent)
    {
        if (loadScene >= numberOfScenes)
        {
            coroutine = loadAs(1,finishEvent);
        }
        else
        {   
            coroutine = loadAs(loadScene,finishEvent);
        }

        StartCoroutine(coroutine);
    }
    public void load(int loadScene)
    {
        if (loadScene >= numberOfScenes)
        {
            coroutine = loadAs(1);
        }
        else
        {
            coroutine = loadAs(loadScene);
        }

        StartCoroutine(coroutine);
    }
    public IEnumerator loadAs(int targetScene)
    {

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        currentScene = targetScene;
    }
    public IEnumerator loadAs(int targetScene,UnityEvent finEvent)
    {

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        currentScene = targetScene;
        finEvent.Invoke();
    }
    public void unloadCurrentScene()
    {
        if(currentScene!=0)
        UnityEngine.SceneManagement.SceneManager.UnloadScene(currentScene);
    }
}
