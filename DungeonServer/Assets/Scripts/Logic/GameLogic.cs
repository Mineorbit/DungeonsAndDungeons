using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic current;
    public int playerCount=1;
    void Start()
    {
        current = this;
    }
    public void Win()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
