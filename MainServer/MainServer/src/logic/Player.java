package logic;

import net.Connector;
import net.pack.Packet;

public class Player {

	public String name;
	//ServerWide ID (Permanent)
	public int globalid;
	public Connector connector;
	public PlayerHandle playerHandle;
	
	Lobby currentLobby = null;
	//ID in lobby(temporary) also important for gameplay range: 0-3
	int localId;
	public boolean setup;
	enum Color {Red, Blue, Green, Yellow};
	Color playerColor;
	
	public Player(String n, Connector info) {
		name = n;
		connector = info;
	}
	
	public void LeaveLobby()
	{
		
	}
	
	public void JoinLobby(Lobby l)
	{
		
	}
	public void JoinLobby(int lobbyId)
	{
		
	}
	public void disconnect() {
		playerHandle.Disconnect();
	}

	public void Update(Packet p) {
		playerHandle.Update(p);
	}

}
