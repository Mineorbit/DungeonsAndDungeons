using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogic : GameLogic
{

    void Start()
    {
        current   =  this;
        Online = false;
        playerCount = 1;
        Pausable = true;
        player    =  Resources.Load("Main/Player/Player");
        playerExt =  Resources.Load("Main/Player/ExternalPlayer");
        playerCamera =  Resources.Load("Main/Player/PlayerCamera");
        
    }
    public void startUnpause()
    {
        Paused = false;
        Time.timeScale = 1;
    }
    public void createPlayer()
    {
    GameObject p = (GameObject) Instantiate(player) as GameObject;
    mainPlayer = p.GetComponent<Player>();
    mainPlayer.Online  = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.current.currentState==GameManager.State.play||GameManager.current.currentState==GameManager.State.test) updateGame();
    }
    public void updateGame()
    {
        updateHUD(); 
    }
    
    public override void  startRound()
    {
        Pausable  = true;
        Paused = false;
        createPlayer();
        spawnLocation = SpawnPointLocation();
        spawnPlayer();
        setupCamera();
        setupLevelRoundStart();
    }
    void setupLevelRoundStart()
    {
        //Setup enemies and such
        GameManager.current.currentLevel.setupRound();
    }

    public void updateHUD()
    {
        float hpfactor = (float)hp/(float)maxhp;
        float mpfactor = (float)mp/(float)maxmp;
        UIManager.current.hpBar.setBar(hpfactor);
        UIManager.current.mpBar.setBar(mpfactor);
    }
    Vector3 SpawnPointLocation()
    {
        float length = 3;
        Vector3 offset = new Vector3(Mathf.Sin((Mathf.PI/2)*(localId-1)),0,-Mathf.Cos((Mathf.PI/2)*(localId-1)));
        offset *= length;
        Vector3 loc = GameManager.current.currentLevel.spawn.transform.position+offset;
        return loc;
    }
    

    
    public void setupCamera()
    {
    camera = Instantiate(playerCamera) as GameObject;
    camera.GetComponent<ThirdPersonCameraController>().target = mainPlayer.transform;
    }

    public void spawnPlayer()
	{
        hp = maxhp;
        mp = maxmp;
        mainPlayer.transform.position = spawnLocation;
	}
    public override void Win()
    {
        UIManager.current.openWin();
        pauseGame();
        Pausable = false;
    }
    public override void stopRound()
    {
        Destroy(mainPlayer.gameObject);
        Destroy(camera);
        Pausable = true;
        unpauseGame();
    }

    public void pauseGame()
    {
        if(Pausable)
        {
    Paused = true;
    Time.timeScale = 0;
    mainPlayer.gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }
    public void unpauseGame()
    {
        if(Pausable)
        {
        Paused = false;
        Time.timeScale = 1;
        mainPlayer.gameObject.GetComponent<PlayerController>().enabled = true;
        }
    }
}
