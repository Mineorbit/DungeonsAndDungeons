using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
    public bool Local;
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
        s = State.Setup;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Debug.Log("Server wird gestartet");
        Server.Start(45565);
    }
    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
