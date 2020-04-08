using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic current;
    public Player mainPlayer;
    public GameObject camera;
    public static GameObject[] players;
	public int hp;
	public int mp;
    public int maxhp  = 100;
	public int maxmp = 100;
    public int localId = 0;

    public int playerCount = 1;

    public bool Paused = false;

    public bool Pausable = true;

    UnityEngine.Object player;
    UnityEngine.Object playerExt;
    UnityEngine.Object playerCamera;

    Vector3 spawnLocation;
    void Start()
    {
        Pausable  = true;
        current   =  this;
        player    =  Resources.Load("Main/Player/Player");
        playerExt =  Resources.Load("Main/Player/ExternalPlayer");
        playerCamera =  Resources.Load("Main/Player/PlayerCamera");
        
    }
    public void startUnpause()
    {
        Paused = false;
        Time.timeScale = 1;
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
    public void startRound()
    {
        //Load Level
        createPlayer();
        spawnLocation = SpawnPointLocation();
        spawnPlayer();
        setupCamera();
    }
    
    public void updateHUD()
    {
        float hpfactor = hp/maxhp;
        float mpfactor = mp/maxmp;
    }
    Vector3 SpawnPointLocation()
    {
        float length = 3;
        Vector3 offset = new Vector3(Mathf.Sin((Mathf.PI/2)*(localId-1)),0,-Mathf.Cos((Mathf.PI/2)*(localId-1)));
        offset *= length;
        Vector3 loc = GameManager.current.currentLevel.spawn.transform.position+offset;
        return loc;
    }
    public void  startTestRound()
    {
        createPlayer();
        spawnLocation = SpawnPointLocation();
        spawnPlayer();
        setupCamera();
    }

    public void createPlayer()
    {
        GameObject p =  Instantiate(player) as GameObject;
        mainPlayer =  p.AddComponent<Player>();
        mainPlayer.id = localId;
    }
    public void createExternalPlayer()
    {

    }
    public void setupCamera()
    {
    camera = Instantiate(playerCamera) as GameObject;
    camera.GetComponent<CameraController>().target = mainPlayer.transform;
    }

    public void spawnPlayer()
	{
        hp = maxhp;
        mp = maxmp;
        mainPlayer.transform.position = spawnLocation;
	}
    public void Win()
    {
        UIManager.current.openWin();
        pauseGame();
        Pausable = false;
    }
    public void stopRound()
    {
        Destroy(mainPlayer.gameObject);
        Destroy(camera);
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
