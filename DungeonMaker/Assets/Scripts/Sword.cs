using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{

    void Start()
    {

        offsetHand   = new Vector3(0,0.23f,-0.1f);
        rotationHand = Quaternion.Euler(0,180,90);
        base.Start();
    }
    void Update()
    {
        base.Update();
    }
    
}
