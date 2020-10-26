using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool acceptInput = true;
    static PlayerController[] playerControllers;
    public void Start()
    {
        playerControllers = new PlayerController[4];

    }
    public static void SetCurrentPlayer(int localId)
    {
        if (localId > 3 || localId < 0) return;
        if(playerControllers[localId]==null)
        {
            playerControllers[localId] = GameObject.Find("Player"+localId).GetComponent<PlayerController>();
        }


        PlayerController.currentPlayer = playerControllers[localId];

        PlayerCameraController.SetTarget(localId);
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
