using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    public override void Start()
    {
        base.Start();
        SetupPlay();
    }
    void SetupPlay()
    {
        gameObject.name = "Player" + localId;
    }
    void Update()
    {
    }
}
