using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLogic : GameLogic
{
    public void createPlayer()
    {
    }
    public override void startRound()
    {
        Pausable =  false;
        createPlayer();
        spawnPlayer();    

        setupCamera();
        setupLevelRoundStart();
    }

    public override void startUnpause()
    {

    }
    public override void stopRound()
    {

    }
    public override Vector3 SpawnPointLocation()
    {
        return new Vector3(0,0,0);  
    }
    public override void spawnPlayer()
    {

    }
    public override void setupCamera()
    {

    }
    public override void setupLevelRoundStart()
    {

    }
    public override void Win()
    {

    }
}
