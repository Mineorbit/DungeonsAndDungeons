using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{
  public void Start()
  {
    rotationHand = Quaternion.Euler(180,270,0);
      base.Start();
  }
  public void Update()
  {
      base.Update();
  }
}
