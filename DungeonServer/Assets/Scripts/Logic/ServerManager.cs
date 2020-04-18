using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
    public bool Local = true;
    public enum State{Setup,Idle,Preparing,Ready,Playing,Won};
    public static State s;
    public Player[] players;

    void Start()
    {
        if(instance==null)
        {
            instance = this;
        }else if(instance!=this)
        {
            Destroy(this);
        }
        SetupServer();
        s = State.Setup;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 64;

        Debug.Log("Server wird gestartet");
        Server.Start(45565);
    }
    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    void SetupServer()
    {
        Directory.CreateDirectory(Application.persistentDataPath+"/map");
    }

    
}
