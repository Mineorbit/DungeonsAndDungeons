using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Player[] players;
    void Start()
    {
        players = new Player[4];
    }

}
