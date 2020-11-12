using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    long levelId = 0;
    public static GameLogic current;
    
    
    public void Awake()
    {
        if (current != null) Destroy(this);
        current = this;
    }

    public static void CheckForWin(bool[] insidePlayers)
    {
        bool win = true;
        bool anyone = false;
        for(int i = 0;i<4;i++)
        {
            if (PlayerManager.Exists(i))
            {
                anyone = true;
                win = win && insidePlayers[i];
            }
        }
        if(win&&anyone)
        ServerManager.instance.performAction(ServerManager.GameAction.WinGame);
    }

    //Called on Victory
    public static void EndRound()
    {
        //Despawn Players
        for(int i = 0;i<4;i++)
        {
            PlayerManager.DespawnPlayer(i);
        }

        if (GameLogic.current != null)
        {
            Destroy(GameLogic.current);
        }
    }

    public static void PrepareRound(Transform t)
    {
        t.gameObject.AddComponent<GameLogic>();
        //Hier war mal ein player spawn ist aber falsch per se
    }

    public static void ClearRound()
    {
        Level.Clear();

        for (int i = 0; i < 4; i++)
        {
            ServerManager.instance.RemoveClient(i);
        }
        EndRound();
    }

    public void StartRound()
    {
        //Reset Player Data if exists
        for (int i = 0; i < 4; i++)
        {
            if (PlayerManager.playerManager.players[i] != null)
                PlayerManager.playerManager.players[i].Reset();
        }
        
        //Set Level As Selected
        LevelData.LevelMetaData levelMetaData = LevelManager.GetSelectedLevel();


        if (levelMetaData == null)
        {
            ServerManager.instance.performAction(ServerManager.GameAction.EndGame); 
        }
        else
        { 

            LevelManager.Load(levelMetaData);

            //Send LevelData

            //and Spawn Players in Positions
            for (int i = 0;i<4;i++)
            {
            Level.currentLevel.SendChunkAt(Level.currentLevel.spawn[i].transform.position, i);
            PlayerManager.SpawnPlayer(i);
            }
        }

    }
    
    
   
    
}
