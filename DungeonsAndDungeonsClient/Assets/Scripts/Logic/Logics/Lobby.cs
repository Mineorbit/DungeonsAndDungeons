using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Lobby : Logic
{
    public static Lobby lobby;

    public Player[] players;
    int localPlayer = 0;
    public Lobby()
    {
        sceneIndex = 1;

        if (lobby == null)
        lobby = this;

        players = new Player[4];
    }
    public void Open(string name)
    {

        players = new Player[4];

        //Open Pop Up with connect
        UnityEvent onConnectEvent = new UnityEvent();
        onConnectEvent.AddListener(OpenLobbyMenu);
        NetworkManager.instance.GameConnect(onConnectEvent,name);

    }
    public void AddLocalPlayer(int localId, string name)
    {
        localPlayer = localId;
        AddPlayer(localId,name);
    }
    public void RemoveLocalPlayer()
    {
        RemovePlayer(localPlayer);
    }

    public void AddPlayer(int localId, string name)
    {
        Player player = new Player();
        player.name = name;
        players[localId] = player;
        PlayerView.playerView.UpdatePlayerView(players);
    }
    public void RemovePlayer(int localId)
    {
        players[localId] = null;
        PlayerView.playerView.UpdatePlayerView(players);
    }
    void OpenLobbyMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromPlayToLobby);
        PlayerView.playerView.UpdatePlayerView(players);
    }


}
