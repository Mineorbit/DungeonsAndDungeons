using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
  public string name;
  public int globalId;
  public int localId;
  public enum Item {Sword = 1,Shield = 2};
  public Item leftHand;
  public Item rightHand;
}
