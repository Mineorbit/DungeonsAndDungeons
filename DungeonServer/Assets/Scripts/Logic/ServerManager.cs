using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
    public bool Local = true;
    public enum State{Setup,Idle,Prepare,Play,GameOver};
    public static State s;
    public Player[] players;
    public static Level currentLevel;

    long levelId = 0;
    void Start()
    {
        if(instance==null)
        {
            instance = this;
        }else if(instance!=this)
        {
            Destroy(this);
        }

        Debug.Log(GetLongBinaryString(levelId));
        Setup();

       
    }

   

    void Idle()
    {
        s = State.Idle;
        //Do house keeping tasks like preloading favorite levels, manage memory etc


    }
    void Play()
    {
        s = State.Play;
        //Block entries communicate to  lobby everything necessary
    }


    void  GameOver()
    {
        s = State.GameOver;
        //Redirect Players and go back to  start

    }

    void Prepare()
    {
        s = State.Prepare;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 64;
        //Download and load map
        
        prepareLevel();

        Debug.Log("Server wird gestartet");
        Server.Start(45565);
    }
    static string GetLongBinaryString(long n)
    {
       byte[] bytes =BitConverter.GetBytes(n); 
       Array.Reverse(bytes);
       string hex = BitConverter.ToString(bytes).Replace("-", string.Empty);
       return hex;
    }

    public void prepareLevel()
    {
        if(!checkLevelAvailable(levelId)) { downloadLevel(levelId); return;}
        loadLevel();
    }

    public void loadLevel()
    {
        GameObject levelHook = GameObject.Find("Level");
		editor.levelHook = levelHook;
		Level level = (levelHook.GetComponent<Level>()==null)?levelHook.AddComponent<Level>():levelHook.GetComponent<Level>();
		level.levelHook = levelHook;
		LevelLoader loader = new LevelLoader();
		LevelData lD = loader.load(levelToLoad);
		level = lD.toLevel(level);
		currentLevel = level;
        currentLevel.setupServerSide();
    }

    public bool checkLevelAvailable(long levelId)
    {
        return true;
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
        //Deallocate everything / delete garbage / create logs
    }
    void Setup()
    {

        s = State.Setup;

        Directory.CreateDirectory(Application.persistentDataPath+"/map");

        if(Local) 
        {
        GameLogic.current.playerCount = 1;
        }
        Prepare();
    }

    
}
