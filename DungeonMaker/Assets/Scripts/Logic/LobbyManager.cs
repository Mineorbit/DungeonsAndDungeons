using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    Stack<LobbyData> invites;
    enum PlayerLobbyState {JoinCreate,PickLevelItem,ModeSet};
    public LobbyManager()
    {
        if(instance == null)
        {instance = this;
        }else Destroy(this);
    }
    public void start()
    {
        openUI();
    }
    public void stop()
    {
        closeUI();
    }
    public void openUI()
    {
        ConnectBar.current.close();
        //LobbyBar.current.Start();
        LobbyBar.current.open();
    }
    public void closeUI()
    {
    LobbyBar.current.close();
    //LobbyBar.current.Start();
    ConnectBar.current.open();
    }

   
}
