using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int localId;
    public string name;
    public enum Color {Blue, Red, Green, Yellow};
    public Color playerColor;

    bool alive = true;
    int health = 100;


    public void Start()
    {

        if(GameManager.GetState() == GameManager.State.Play)
        {
            SetupPlay();
        }

    }

    void SetupPlay()
    {
    gameObject.name = "Player" + localId;
    }


    public virtual void  Spawn(Vector3 location,Quaternion rotation, bool allowedToMove)
    {
        health = 100;
        alive = true;
        PlayerManager.playerManager.SpawnPlayer(localId, location);
    }

    public void setMovementStatus(bool allowedToMove)
    {
        PlayerManager.playerManager.playerControllers[localId].allowedToMove = allowedToMove;
    }

    public virtual void Kill()
    {
        health = 0;
        alive = false;
        PlayerManager.playerManager.DespawnPlayer(localId);
    }


    public void Update()
    {
    }
}
