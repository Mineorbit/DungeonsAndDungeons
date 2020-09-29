using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerView : MonoBehaviour
{
    public static PlayerView playerView;
    Player[] playerData;
    PlayerElement[] playerElements;
    void Start()
    {
        if (playerView != null) Destroy(this);
        playerView = this;


        playerData = new Player[4];
        playerElements = new PlayerElement[4];
        for(int i = 0;i<playerElements.Length;i++)
        {
            playerElements[i] = transform.Find(i.ToString()).GetComponent<PlayerElement>();
        }
    }

    public void UpdatePlayerView(Player[] playerInfo)
    {
        Player[] playerInfos = new Player[4];
        Array.Copy(playerInfo,0,playerInfos,0,playerInfo.Length);
        playerData = playerInfos;
        for(int i = 0; i < playerElements.Length; i++)
        {
            playerElements[i].UpdateElement(playerData[i]);
        }
    }
}
