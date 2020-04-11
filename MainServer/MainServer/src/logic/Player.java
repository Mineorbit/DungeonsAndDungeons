package logic;

import net.Connector;
import net.pack.Packet;

public class Player {

	public String name;
	// ServerWide ID (Permanent)
	public int globalid;
	public Connector connector;
	public PlayerHandle playerHandle;

	Lobby currentLobby = null;
	// ID in lobby(temporary) also important for gameplay range: 0-3
	int localId;
	public boolean setup;

	enum Color {
		Red, Blue, Green, Yellow
	};

	Color playerColor;

	public Player(String n, Connector info) {
		name = n;
		connector = info;
	}

	public void LeaveLobby() {

	}

	public void JoinLobby(Player joinee, Lobby l) {
		// Should only be possible if invite packet has been sent to player before
	}

	public void JoinLobby(Player joinee, int lobbyId) {

	}

	public void disconnect() {

		// Removal from all Lobbys and Server data

		// Disconnect
		playerHandle.Disconnect();
	}

	// Any Information (Will be displayed in Overlay/NetworkInfo)
	public void sendNotification(String message) {
		NotificationPacket p = new NotificationPacket();
		playerHandle.Update(p);
	}

}
