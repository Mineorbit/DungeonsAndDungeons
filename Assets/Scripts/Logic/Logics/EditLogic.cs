using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class EditLogic : Logic
{
    private GameObject builder;

    public override void Init()
    {
        sceneIndex = 3;
    }

    public override void Start()
    {
        if (running) return;
        base.Start();
        Debug.Log("Starting Edit");

        PlayerManager.DeactivateAllPlayers();
    }

    public override void Stop()
    {
        if (!running) return;
        base.Stop();
        Debug.Log("Stopping Edit");
    }

    public override void DeInit()
    {
    }
}