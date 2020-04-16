using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public bool Host;
    void  Start()
    {

    }
    void FixedUpdate()
    {
        if(Host)
        {
            //Send Data
        }
    }
}
