using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{

    void Start()
    {
        useTime = 0.2f;
        base.Start();
    }
    void Update()
    {

        base.Update();
    }

    public override void animateDefault()
    {
        offsetHand   = new Vector3(-0.25f,0.35f,0.0475f);
        rotationHand = Quaternion.Euler(0,270,90);
        base.animateDefault();
    }
    public override void animateAction()
    {
        offsetHand   = new Vector3(-0.25f,0.35f,0.0475f);
        rotationHand = Quaternion.Euler(0,270,90);
        base.animateAction();
    }
}
