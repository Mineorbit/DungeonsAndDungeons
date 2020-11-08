using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static bool acceptInput = true;
    public static PlayerManager playerManager;
    public PlayerController[] playerControllers;
    public Player[] players;

    public static int currentPlayerLocalId;

    public void Start()
    {
        if(playerManager!=null) Destroy(this);
        playerManager= this;
        playerControllers = new PlayerController[4];
        players = new Player[4];
    }

    public void Update()
    {
        void Update()
        {
            if (GameManager.GetState() == GameManager.State.Play || GameManager.GetState() == GameManager.State.Test)
            {
                PlayerController.doSim = true;
            }
            else
            {
                PlayerController.doSim = false;
            }
        }
    }

    public void Reset()
    {
        for(int i = 0;i<4;i++)
        {
           Remove(i);
        }
    }


    public void SetCurrentPlayer(int localId)
    {
        if (localId > 3 || localId < 0) return;
        Debug.Log(localId);
        if(playerControllers[localId]==null)
        {
            playerControllers[localId] = GameObject.Find("Player"+localId).GetComponent<PlayerController>();
            players[localId] = GameObject.Find("Player" + localId).GetComponent<Player>();
        }


        PlayerController.currentPlayer = playerControllers[localId];


        currentPlayerLocalId = localId;
    }





    public void Remove(int localId)
    {
        if (localId > 3 || localId < 0) return;
        if(playerControllers[localId]!=null)
        if (playerControllers[localId].gameObject!=null)

        Destroy(playerControllers[localId].gameObject);

        if(currentPlayerLocalId == localId)
        {
            PlayerController.currentPlayer = null;
            currentPlayerLocalId = -1;
        }

    }


    public void Add(int freeLocalId, string name)
    {

        InstantionTarget t = Resources.Load("pref/lobby/data/Player") as InstantionTarget;
        GameObject g = t.Create(new Vector3(32 + freeLocalId * 8, 6, 0), transform);

        Player player = g.AddComponent<Player>();
        player.name = name;
        player.localId = freeLocalId;
        players[freeLocalId] = player;
        playerControllers[freeLocalId] = g.GetComponent<PlayerController>();

    }

    public void DespawnPlayer(int localId)
    {
        if (localId > 3 || localId < 0) return;
        if(playerControllers[localId]!= null)
        playerControllers[localId].gameObject.SetActive(false);

    }

    public void SpawnPlayer(int localId,Vector3 location)
    {
        if (localId > 3 || localId < 0) return;


        if (playerControllers[localId] == null) return;


            playerControllers[localId].gameObject.SetActive(true);
        //Move to other class Player eventually
        playerControllers[localId].transform.position = location;
        //Noch HUD Aktivieren
    }
}
