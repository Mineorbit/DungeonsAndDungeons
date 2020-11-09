using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLogic : Logic
{
    public override void Init()
    {
        sceneIndex = 4;
        created = Instantiator.InstantiateAssets("play");
    }

    public bool SpawnPositionSet;
    public bool SpawnChunkReceived;
    public bool CheckReady()
    {
        return SpawnPositionSet && SpawnChunkReceived;
    }
    public override void Start()
    {
        if (running) return;

        if (!CheckReady()) return;

        base.Start();

        PlayerManager.playerManager.SetCurrentPlayer(NetworkManager.instance.localId);
        LoadingScreen.instance.Close();

    }

}
