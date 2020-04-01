using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{
  public void Start()
  {
      rotationHand = Quaternion.Euler(0f,-90f,0f);
      offsetHand    = new Vector3(0,0,-1);
      base.Start();
  }
  public void Update()
  {
      base.Update();
  }
}
