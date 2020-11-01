using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int localId;
    public string name;

    public bool ready;

    Vector3 targetPosition;
    Quaternion targetRotation;


    public Client client;

    void Start()
    {
        Setup();
    }
    public void Setup()
    {
        ready = false;
        PlayerManager.playerManager.AddPlayer(this,localId);
    }
    void Update()
    {
        transform.position = Vector3.Lerp(targetPosition,transform.position,0.5f);
        transform.rotation = Quaternion.Lerp(targetRotation,transform.rotation,0.5f);
    }

    void FixedUpdate()
    {
        if(targetPosition!=transform.position)
        {
        //ServerSend.PlayerLocomotionData(id,targetPosition,targetRotation);
        }
    }
    public void updateLocomotionData(Vector3 loc,Quaternion rot)
    {
        Debug.Log("Changing target "+localId);
        targetPosition = loc;
        targetRotation = rot;
    }
}
