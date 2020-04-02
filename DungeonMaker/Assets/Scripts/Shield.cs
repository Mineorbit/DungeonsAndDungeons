using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{

  public void Start()
  {
      animateDefault();
      base.Start();
  }
  public void Update()
  {
      base.Update();
  }
  public override void animateDefault()
  {
    offsetHand   = new Vector3(0,0,-0.26f);
    rotationHand = Quaternion.Euler(180,270,0);
  }
  public override void animateAction()
  {
    offsetHand   = new Vector3(0,0.385f,-0.12f);
    rotationHand = Quaternion.Euler(10,180,250);
  }
}
