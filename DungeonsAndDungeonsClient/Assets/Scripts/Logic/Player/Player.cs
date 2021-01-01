using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController playerController;
    public ColorChanger colorChanger;
    public int _localId;

    public float speed;

    public int localId
    {
        set { _localId = value;
            changeColor(_localId);
        }
        get { return _localId; }
    }


    public string name;
    public enum Color {Blue, Red, Green, Yellow};
    public Color playerColor;

    bool alive = true;
    int health = 100;
    public virtual void Awake()
    {
        colorChanger = gameObject.GetComponent<ColorChanger>();
    }
    public virtual void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }

    public void changeColor(int id)
    {
        switch(id)
        { 
            case 0:
                playerColor = Color.Blue;
                setColor(UnityEngine.Color.blue);
                break;
            case 1:
                playerColor = Color.Yellow;
                setColor(UnityEngine.Color.yellow);
                break;
            case 2:
                playerColor = Color.Red;
                setColor(UnityEngine.Color.red);
                break;
            case 3:
                playerColor = Color.Green;
                setColor(UnityEngine.Color.green);
                break;
        }
    }
    void setColor(UnityEngine.Color baseC)
    {

        colorChanger.SetColor(7, baseC);
        colorChanger.SetColor(2, colorChanger.comp(baseC));
        colorChanger.SetColor(8,UnityEngine.Color.Lerp(baseC,UnityEngine.Color.white,0.75f));
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
