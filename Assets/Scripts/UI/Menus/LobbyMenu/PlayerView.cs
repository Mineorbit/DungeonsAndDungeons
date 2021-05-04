using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using com.mineorbit.dungeonsanddungeonscommon;

public class PlayerView : MonoBehaviour
{
    public static PlayerView playerView;
    PlayerElement[] playerElements;
    void Start()
    {
        if (playerView != null) Destroy(this);
        playerView = this;


        playerElements = new PlayerElement[4];
        for(int i = 0;i<playerElements.Length;i++)
        {
            playerElements[i] = transform.Find(i.ToString()).GetComponent<PlayerElement>();
        }
    }


    public void UpdatePlayerView()
    {
        for (int i = 0; i < playerElements.Length; i++)
        {
        playerElements[i].UpdateElement(PlayerManager.playerManager.players[i]);
        }
    }

    public void Update()
    {
        UpdatePlayerView();
    }
}
