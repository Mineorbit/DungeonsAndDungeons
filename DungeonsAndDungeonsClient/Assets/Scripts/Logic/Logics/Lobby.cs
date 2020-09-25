using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public static Lobby lobby;

    public Player[] players;
    void Start()
    {
        if (lobby != null) Destroy(this);
        lobby = this;
        players = new Player[4];
    }
    public void Open(string name)
    {
        Player player = new Player();
        player.name = name;
        Lobby.lobby.players[0] = player;
        //Open Pop Up with connect
    }

}
