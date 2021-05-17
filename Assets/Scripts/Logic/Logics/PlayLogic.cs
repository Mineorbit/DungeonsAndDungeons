using com.mineorbit.dungeonsanddungeonscommon;

public class PlayLogic : Logic
{
    public static bool spawnPositionSet;
    public static bool spawnChunkReceived;

    public static bool SpawnChunkReceived
    {
        set
        {
            spawnChunkReceived = value;
            CheckReady();
        }
        get => spawnChunkReceived;
    }

    public static bool SpawnPositionSet
    {
        set
        {
            spawnPositionSet = value;
            CheckReady();
        }
        get => spawnPositionSet;
    }

    public override void Init()
    {
        sceneIndex = 4;
        created = Instantiator.InstantiateAssets("play");
    }

    public static void CheckReady()
    {
        if (spawnPositionSet && spawnChunkReceived)
            if (GameManager.instance.currentLogic.GetType() == typeof(PlayLogic))
                GameManager.instance.currentLogic.Start();
    }

    public override void Start()
    {
        if (running) return;

        base.Start();
        LoadingScreen.instance.Close();
        PlayerManager.playerManager.SetCurrentPlayer(NetworkManager.instance.localId);

        PlayerController.doSim = true;
    }
}