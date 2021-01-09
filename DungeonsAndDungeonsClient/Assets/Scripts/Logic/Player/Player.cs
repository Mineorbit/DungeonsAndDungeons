using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController playerController;

    public Item[] items;
    public ItemHandle[] itemHandles;

    public ColorChanger colorChanger;

    public int _localId;

    public float speed;



    bool hitCooldown = false;

    public int localId
    {
        set
        {
            _localId = value;
            changeColor(_localId);
        }
        get { return _localId; }
    }


    public string name;
    public enum Color { Blue, Red, Green, Yellow };
    public Color playerColor;

    bool alive = true;
    public int health = 100;
    float cooldownTime = 2f;
 
    public virtual void Awake()
    {
        colorChanger = gameObject.GetComponent<ColorChanger>();
    }
    public virtual void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        itemHandles = gameObject.GetComponentsInChildren<ItemHandle>();
        items = gameObject.GetComponentsInChildren<Item>();

        itemHandles[0].Attach(items[0]);
    }

    public virtual bool IsAlive()
    {
        return alive;
    }

    public void changeColor(int id)
    {
        switch (id)
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
        colorChanger.SetColor(8, UnityEngine.Color.Lerp(baseC, UnityEngine.Color.white, 0.75f));
    }

    public virtual void Spawn(Vector3 location, Quaternion rotation, bool allowedToMove)
    {
        health = 100;
        alive = true;
        PlayerManager.playerManager.SpawnPlayer(localId, location);
    }

    public void setMovementStatus(bool allowedToMove)
    {
        PlayerManager.playerManager.playerControllers[localId].allowedToMove = allowedToMove;
    }


    IEnumerator HitTimer(float time)
    {
        yield return new WaitForSeconds(time);
        hitCooldown = false;
    }


    void StartHitCooldown()
    {

        hitCooldown = true;
        StartCoroutine(HitTimer(cooldownTime));
    }
    
    public virtual void Hit(int damage)
    {
        if(!hitCooldown)
        {
        StartHitCooldown();
        health = health - damage;
        if(health == 0)
        {
            Kill();
        }
        }

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
