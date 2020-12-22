using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    public void Start()
    {
        SetupPlay();
    }
    void SetupPlay()
    {
        gameObject.name = "Player" + localId;
    }

}
