package logic;

import main.Server;
import net.Connector;
import net.pack.LengthPacket;
import net.pack.NotificationPacket;

public class Player {
	private String name;
	// ServerWide ID (Permanent)
	public int globalID;
	public Connector connector;
	public PlayerHandle playerHandle;

	Lobby currentLobby = null;
	// ID in lobby(temporary) also important for gameplay range: 0-3
	int localId;
	public boolean setup;

	private PlayerColor playerColor;

	public Player(String n, Connector info) {
		setName(n);
		connector = info;
	}

	public void leaveLobby() {

	}

	public void joinLobby(Player joinee, Lobby l) {
		// Should only be possible if invite packet has been sent to player before
	}

	public void joinLobby(Player joinee, int lobbyId) {

	}

	public void disconnect() {
		// Removal from all Lobbys and Server data
		Server server = Server.getInstance();
		synchronized (server) {
			
		}

		// Disconnect
		playerHandle.disconnect();
	}

	// Any Information (Will be displayed in Overlay/NetworkInfo)
	public void sendNotification(String message) {
		NotificationPacket noti = new NotificationPacket(message);
		LengthPacket p = new LengthPacket(noti);
		playerHandle.send(p);
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public PlayerColor getPlayerColor() {
		return playerColor;
	}

	public void setPlayerColor(PlayerColor playerColor) {
		this.playerColor = playerColor;
	}

}
