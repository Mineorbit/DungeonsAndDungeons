using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    PlayerData[] playerDataInLobby;
    public PlayerListItem[] playerPreview;
    // Start is called before the first frame update
    void Start()
    {
        playerDataInLobby = new PlayerData[4];
        playerPreview = new PlayerListItem[4];
        for(int i = 0;i<4;i++)
        {
            playerPreview[i] = transform.Find(""+i).Find("PlayerListItem").GetComponent<PlayerListItem>();
            playerPreview[i].close();
        }
        startSoloMode();
    }

    void startSoloMode()
    {

    }
    void startMultiPlayerMode()
    {

    }
    public void AddPlayer(PlayerData data)
    {

    }


    public void RemovePlayer(int localId)
    {

    }
    public void ChangePlayerData(int localId, PlayerData newData)
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
