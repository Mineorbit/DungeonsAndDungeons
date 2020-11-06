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

    bool SpawnPositionSet;
    bool SpawnChunkReceived;
    public bool CheckReady()
    {
        return SpawnPositionSet && SpawnChunkReceived;
    }
    public override void Start()
    {
        if (running) return;
        base.Start();

        if (!CheckReady()) return;

        Debug.Log("Hussa wir sind im Modus");

        GameManager.instance.CompletePlayLoad();

        PlayerManager.playerManager.SetCurrentPlayer(NetworkManager.instance.localId);
    }

}
