using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLogic : GameLogic
{
   
   
    public void Start()
    {
        current = this;
        localId = Client.instance.localId;
        players = new GameObject[4];
        playerComps = new Player[4];
        player = Resources.Load("Main/Player/Player");
        playerExt = Resources.Load("Main/Player/ExternalPlayer");
        playerCamera = Resources.Load("Main/Player/PlayerCamera");
    }
    public override void startRound()
    {
        Debug.Log("Round start");
        Pausable = true;

        setupPlayers();

        setupCamera();
        setupLevelRoundStart();
		GameManager.current.closeLoadingScreen();
    }

    public override void startUnpause()
    {

    }
    public override void stopRound()
    {

    }
    
    public override Vector3 SpawnPointLocation()
    {
        if(GameManager.current.currentLevel==null)
        {
            return new Vector3(20,20,20);
        }else
        {
            return GameManager.current.currentLevel.spawn.transform.position+new Vector3(0,5,0);
        }  
    }
    public void setupPlayers()
    {
        for(int i = 0;i<4;i++)
        {
            Vector3 offset = new Vector3(Mathf.Sin((Mathf.PI/4)*(float)i),0,-Mathf.Cos((Mathf.PI/4)*(float)i));
            
            offset *=3;
            Debug.Log(i+" "+offset);
            if(GameManager.current.playerData[i]!=null)
            {
                GameObject p;
                Player pp;
                if(i==Client.instance.localId)
                {
                    p = (GameObject) Instantiate(player) as GameObject;
                    pp = p.GetComponent<Player>();
                    pp.Host = true;
                    pp.id = i;
                    mainPlayer = pp;

                }else
                {
                    //Spawn ExtPlayer
                    p = (GameObject) Instantiate(playerExt) as GameObject;
                    pp = p.GetComponent<Player>();
                    pp.Host = false;
                    pp.id = i;
                }
                players[i] = p;
                playerComps[i] = pp;
                p.transform.position = SpawnPointLocation()+offset;

            }
        }
    }


    public override void setupCamera()
    {
        Debug.Log("Test");
        GameObject camera = (GameObject) Instantiate(playerCamera) as GameObject;
        camera.transform.position = SpawnPointLocation();
        camera.GetComponent<CameraController>().target = players[Client.instance.localId].transform;
    }
    public override void setupLevelRoundStart()
    {

    }
    public override void Win()
    {

    }
}
