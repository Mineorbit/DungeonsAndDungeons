using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Player[] players;
    public static int playerCount;
    public void Awake()
    {
        players = new Player [4];
    }
    public static void setupPlayer(int localId)
    {

    }
    public static void destroyPlayer(int localId)
    {

    }
}
