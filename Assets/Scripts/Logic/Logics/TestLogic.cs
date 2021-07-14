using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class TestLogic : Logic
{
    private int player;

    public override void Init()
    {
        sceneIndex = 2;
    }

    private void CreatePlayers()
    {
        for (var i = 0; i < 4; i++) PlayerManager.playerManager.Add(i, "Rot", true);
    }

    public override void Start()
    {
        if (running) return;
        base.Start();
        LevelManager.StartRound(false);

        PlayerGoal.GameWinEvent.AddListener(
            () => { GameManager.instance.performAction(GameManager.EnterEditFromTest); });

        SpawnAll();
        CreatePlayers();
        PlayerManager.playerManager.StartRound();
        PlayerManager.playerManager.SetCurrentPlayer(player);
    }


    //This is ugly need better way
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Swap();
    }

    public void Swap()
    {
        player = (player + 1) % 4;
        PlayerManager.playerManager.SetCurrentPlayer(player);
    }

    private Vector3 GetSpawnLocation(int i)
    {
        if (LevelManager.currentLevel.spawn[i] != null)
            return LevelManager.currentLevel.spawn[i].transform.position + new Vector3(0, 1.5f, 0);
        return new Vector3(i * 4, 0.25f, 0);
    }


    private void RemovePlayers()
    {
        for (var i = 0; i < 4; i++) PlayerManager.playerManager.Remove(i);
    }

    public override void Stop()
    {
        if (!running) return;

        RemovePlayers();
        DespawnAll();


        LevelManager.EndRound(false);

    }

    public override void DeInit()
    {
    }
}