using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SceneLoadManager : MonoBehaviour
{
    private int numberOfScenes = 3;  
    public int[] currentScene;
    private IEnumerator coroutine;
    public static SceneLoadManager instance;
    public void Awake()
    {
        if(instance!=null)
        {
            Destroy(this);
        }
        instance = this;

        currentScene = new int[1];
        currentScene[0] = 0;
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
    public void load(int[] loadScene, UnityEvent finishEvent)
    {
        if (loadScene[0] >= numberOfScenes)
        {
            coroutine = loadAs(1, finishEvent);
        }
        else
        {
            coroutine = loadAs(loadScene, finishEvent);
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
    public void load(int[] loadScene)
    {
        if (loadScene[0] >= numberOfScenes)
        {
            coroutine = loadAs(1);
        }
        else
        {
            coroutine = loadAs(loadScene[0]);
        }

        StartCoroutine(coroutine);
    }
    public IEnumerator loadAs(int[] targetScenes)
    {
        foreach(int targetScene in targetScenes)
        { 
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        }
        currentScene = targetScenes;
    }
    public IEnumerator loadAs(int[] targetScenes,UnityEvent finEvent)
    {
        foreach (int targetScene in targetScenes)
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        currentScene = targetScenes;
        finEvent.Invoke();
    }
    public IEnumerator loadAs(int targetScene)
    {

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        int[] ret = new int[1];
        ret[0] = targetScene;
        currentScene = ret;
    }
    public IEnumerator loadAs(int targetScene,UnityEvent finEvent)
    {

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        int[] ret = new int[1];
        ret[0] = targetScene;
        currentScene = ret;
        finEvent.Invoke();
    }
    public void unloadCurrentScene()
    {
        if(currentScene[0]!=0)
        UnityEngine.SceneManagement.SceneManager.UnloadScene(currentScene[0]);
    }
    public void unloadCurrentScenes()
    {
        for(int i = 0;i < currentScene.Length;i++)
            if (currentScene[i] != 0)
                UnityEngine.SceneManagement.SceneManager.UnloadScene(currentScene[i]);
    }
}
