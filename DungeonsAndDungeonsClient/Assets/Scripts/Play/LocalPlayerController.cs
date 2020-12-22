using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

//Only in state Test or PlayLocal (Local Play)
public class LocalPlayerController : PlayerController
{

    LocalPlayer player;

    void Update()
    {
        UpdateGround();
        TestUpdate();
        Move();
    }
    void TestUpdate()
    {
        doSim = true;

        doInput = PlayerManager.acceptInput && allowedToMove; //&& !player.lockNetUpdate;
    }

}
