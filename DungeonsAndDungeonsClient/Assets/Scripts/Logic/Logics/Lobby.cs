using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

        players = new Player[4];

        Player player = new Player();
        player.name = name;
        Lobby.lobby.players[0] = player;
        //Open Pop Up with connect
        UnityEvent onConnectEvent = new UnityEvent();
        onConnectEvent.AddListener(OpenLobbyMenu);
        NetworkManager.instance.LobbyConnect(onConnectEvent,name);

    }
    
    public void Close()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.GoBack);
    }
    void OpenLobbyMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromPlayToLobby);
        PlayerView.playerView.UpdatePlayerView(players);
    }


}
