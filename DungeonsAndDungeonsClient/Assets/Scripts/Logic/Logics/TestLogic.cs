using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogic : Logic
{

    GameObject[] created;
    GameObject player;
    public override void Init()
    {
        Debug.Log("Init Test");
        created = Instantiator.InstantiateAssets("test");
        player = created[0];
    }

    public override void Start()
    {
        if (running) return;
        base.Start();
        SpawnPlayer(new Vector3(0, 0, 0));
    }
    public override void Stop()
    {
        if (!running) return;
        DespawnPlayer();
    }
    void DespawnPlayer()
    {
        player.SetActive(false);
    }
    void SpawnPlayer(Vector3 location)
    {
        player.transform.position = location;
        player.SetActive(true);
        //Noch HUD Aktivieren
    }
}
