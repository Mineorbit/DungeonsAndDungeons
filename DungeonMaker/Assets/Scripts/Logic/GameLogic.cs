using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic current;
    public int playerCount;
    public bool Pausable;
    public bool Online;
    public bool Paused = false;

    public Player mainPlayer;
    public GameObject camera;
    public static GameObject[] players;
	public int hp = 50;
	public int mp =  50;
    public int maxhp  = 100;
	public int maxmp = 100;
    public int localId = 0;


    public Vector3 spawnLocation;

    public UnityEngine.Object player;
    public UnityEngine.Object playerExt;
    public UnityEngine.Object playerCamera;

    void Start()
    {
        Debug.Log("Testing GameLogic setup");
        players = new GameObject[4];
    }
    public virtual void createPlayer()
    {
    }
    public virtual void startRound()
    {
        if(!Online) Pausable  = true;
        Paused = false;
        createPlayer();
        spawnLocation = SpawnPointLocation();
        spawnPlayer();    

        setupCamera();
        setupLevelRoundStart();
    }

    public virtual void startUnpause()
    {

    }
    public virtual void stopRound()
    {

    }
    public virtual Vector3 SpawnPointLocation()
    {
        return new Vector3(0,0,0);  
    }
    public virtual void spawnPlayer()
    {

    }
    public virtual void setupCamera()
    {

    }
    public virtual void setupLevelRoundStart()
    {

    }
    public virtual void Win()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
