using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static bool acceptInput = true;
    public static PlayerController[] playerControllers;
    public static Player[] players;

    static int currentPlayerLocalId;

    public void Start()
    {
        playerControllers = new PlayerController[4];
        players = new Player[4];
    }

    public static void SetCurrentPlayer(int localId)
    {
        if (localId > 3 || localId < 0) return;
        Debug.Log(localId);
        if(playerControllers[localId]==null)
        {
            playerControllers[localId] = GameObject.Find("Player"+localId).GetComponent<PlayerController>();
            players[localId] = GameObject.Find("Player" + localId).GetComponent<Player>();
        }

        Debug.Log(playerControllers[localId]);

        PlayerController.currentPlayer = playerControllers[localId];

        PlayerCameraController.SetTarget(localId);

        currentPlayerLocalId = localId;
        Debug.LogError("Active Player: "+localId);
    }





    public static void Remove(int localId)
    {
        if (localId > 3 || localId < 0) return;
        if(playerControllers[localId]!=null)
        if (playerControllers[localId].gameObject!=null)
        Destroy(playerControllers[localId].gameObject);

        if(currentPlayerLocalId == localId)
        {
            PlayerController.currentPlayer = null;
            PlayerCameraController.SetTarget(-1);
            currentPlayerLocalId = -1;
        }

    }


    public static int Add(PlayerController playerController)
    {
        int i = 0;
        while(playerControllers[i] != null && i < playerControllers.Length)
        {
            i++;
        }
        if(playerControllers[i] == null)
        {
            playerControllers[i] = playerController;
            players[i] = playerController.gameObject.GetComponent<Player>();
            return i;
        }else
        {
            return -1;
        }
    }

    public static void DespawnPlayer(int localId)
    {
        if (localId > 3 || localId < 0) return;
        if(playerControllers[localId]!= null)
        playerControllers[localId].gameObject.SetActive(false);

    }

    public static void SpawnPlayer(int localId,Vector3 location)
    {
        if (localId > 3 || localId < 0) return;


        if (playerControllers[localId] == null) return;

            playerControllers[localId].gameObject.SetActive(true);
        //Move to other class Player eventually
        playerControllers[localId].transform.position = location;
        //Noch HUD Aktivieren
    }
}
