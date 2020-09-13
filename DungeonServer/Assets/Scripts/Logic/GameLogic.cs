using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic current;
    public void Start()
    {
        if (current != null) Destroy(this);
        current = this;
    }
    
}
