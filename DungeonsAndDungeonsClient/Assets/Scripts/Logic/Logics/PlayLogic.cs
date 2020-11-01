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

    public override void Start()
    {
        if (running) return;
        base.Start();

        PlayerManager.playerManager.SetCurrentPlayer(NetworkManager.instance.localId);
    }
}
