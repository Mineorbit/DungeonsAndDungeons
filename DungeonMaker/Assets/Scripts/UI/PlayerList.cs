using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    PlayerData[] playerDataInLobby;
    // Start is called before the first frame update
    void Start()
    {
        playerDataInLobby = new PlayerData[4];
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
