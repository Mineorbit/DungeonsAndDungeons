using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Lobby : Logic
{
    public static Lobby lobby;

    public Player[] players;
    int localPlayer = 0;

    public override void Init()
    {

        sceneIndex = 1;

        if (lobby == null)
            lobby = this;

    }
    public override void Start()
    {

        players = new Player[4];
    }

    public void Open(string name)
    {


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
        GameObject pM = GameObject.Find("PlayerManager");
        InstantionTarget t = Resources.Load("pref/play/data/Player") as InstantionTarget;
        GameObject g = t.Create(new Vector3(0,0,0) ,pM.transform);

        Player player = g.AddComponent<Player>();
        player.name = name;
        players[localId] = player;

        PlayerManager.Add(g.GetComponent<PlayerController>());


        PlayerView.playerView.UpdatePlayerView(players);
    }
    public void RemovePlayer(int localId)
    {
        PlayerManager.Remove(localId);
        Instantiator.Remove(players[localId].gameObject);
        PlayerView.playerView.UpdatePlayerView(players);
    }
    void OpenLobbyMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromPlayToLobby);
        PlayerView.playerView.UpdatePlayerView(players);
    }


}
