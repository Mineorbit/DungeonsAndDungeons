﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public enum SceneIndex
    {
        main = 0,
        menu,
        test,
        edit,
        play
    }

    public static SceneLoadManager instance;
    public List<int> currentScenes;

    private IEnumerator coroutine;
    private readonly int numberOfScenes = 5;

    public void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;

        currentScenes = new List<int>();
        currentScenes.Add(0);
    }

    public static void SetSceneState(SceneIndex index, bool active)
    {
        var testScene = SceneManager.GetSceneByBuildIndex((int) index);
        var testObjects = testScene.GetRootGameObjects();
        foreach (var g in testObjects) g.SetActive(active);
    }

    /*
    public void load(int loadScene)
    {
        if (loadScene >= numberOfScenes)
        { UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Additive); }else
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
    */
    public void load(int loadScene, UnityEvent finishEvent)
    {
        if (loadScene >= numberOfScenes)
            coroutine = loadAs(1, finishEvent);
        else
            coroutine = loadAs(loadScene, finishEvent);

        StartCoroutine(coroutine);
    }

    public void load(int[] loadScene, UnityEvent finishEvent)
    {
        if (loadScene[0] >= numberOfScenes)
            coroutine = loadAs(1, finishEvent);
        else
            coroutine = loadAs(loadScene, finishEvent);

        StartCoroutine(coroutine);
    }

    public void load(SceneIndex index, UnityEvent finishEvent)
    {
        load((int) index, finishEvent);
    }

    public void load(SceneIndex[] indices, UnityEvent finishEvent)
    {
        var indicesInt = new int[indices.Length];
        for (var i = 0; i < indices.Length; i++) indicesInt[i] = (int) indices[i];
        load(indicesInt, finishEvent);
    }


    public void load(SceneIndex index)
    {
        load((int) index);
    }

    public void load(int loadScene)
    {
        if (loadScene >= numberOfScenes)
            coroutine = loadAs(1);
        else
            coroutine = loadAs(loadScene);

        StartCoroutine(coroutine);
    }

    public void load(int[] loadScene)
    {
        if (loadScene[0] >= numberOfScenes)
            coroutine = loadAs(1);
        else
            coroutine = loadAs(loadScene[0]);

        StartCoroutine(coroutine);
    }

    public IEnumerator loadAs(int[] targetScenes)
    {
        foreach (var targetScene in targetScenes)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
            while (!asyncLoad.isDone) yield return null;
        }

        currentScenes.AddRange(targetScenes.ToList());
    }

    public IEnumerator loadAs(int[] targetScenes, UnityEvent finEvent)
    {
        foreach (var targetScene in targetScenes)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
            while (!asyncLoad.isDone) yield return null;
        }

        currentScenes.AddRange(targetScenes.ToList());
        finEvent.Invoke();
    }

    public IEnumerator loadAs(int targetScene)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) yield return null;
        currentScenes.Add(targetScene);
    }

    public IEnumerator loadAs(int targetScene, UnityEvent finEvent)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) yield return null;

        currentScenes.Add(targetScene);
        finEvent.Invoke();
    }

    public void unloadCurrentScene()
    {
        if (currentScenes.Count > 0)
        {
            var x = currentScenes[0];
            if (x > 0)
            {
                GameConsole.Log("Unloading " + x);
                SceneManager.UnloadScene(x);
                currentScenes.RemoveAt(0);
            }
        }
    }


    public void unload(int i)
    {
        if (currentScenes.Contains(i))
            if (i > 0)
            {
                currentScenes.Remove(i);
                SceneManager.UnloadScene(i);
            }
    }

    public void unloadCurrentScenes()
    {
        var toUnload = new List<int>(currentScenes);
        foreach (var x in toUnload)
            if (x > 0)
            {
                Debug.Log("Unloading " + x);
                SceneManager.UnloadScene(x);
                currentScenes.Remove(x);
            }
    }
}