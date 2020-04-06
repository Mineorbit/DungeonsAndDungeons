using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{
  bool overrideIt = false;
  public void Start()
  {
      animateDefault();
      base.Start();
  }
  public void Update()
  {
      base.Update();
      if(overrideIt)
      overrideAnimate();
  }
  public override void animateDefault()
  {
    overrideIt = false;
    offsetHand   = new Vector3(0.2f,0.22f,-0.165f);
    rotationHand = Quaternion.Euler(120,-12,160);
    base.animateDefault();
  }
  public override void animateAction()
  {
    Debug.Log("Test");
    overrideIt = true;
    offsetHand   = new Vector3(0.1f,0.385f,-0.12f);
    rotationHand = transform.parent.rotation*Quaternion.Euler(120,-12,160);
  }
  void overrideAnimate()
  {
    transform.localPosition = offsetHand;
    transform.rotation = rotationHand;
  }
}
