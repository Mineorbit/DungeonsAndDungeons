package logic;

import main.Server;
import net.Connector;
import net.pack.LengthPacket;
import net.pack.NotificationPacket;

public class Player {
	private String name;
	// ServerWide ID (Permanent)
	private int globalID;
	private Connector connector;
	public PlayerHandle playerHandle;

	private Lobby currentLobby;

	// ID in lobby(temporary) also important for gameplay range: 0-3
	private int localId;
	private PlayerColor playerColor;

	public Player(int globalId, String n, Connector info) {
		this.globalID = globalId;
		this.name = n;
		this.currentLobby = null;
		connector = info;
	}

	public void leaveLobby() {
		synchronized (currentLobby) {
			currentLobby.removePlayer(localId);
		}
	}
	
	public void disconnect() {
		// Removal from all Lobbys and Server data
		synchronized (currentLobby) {
			currentLobby.removePlayer(localId);
		}
		
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

	public int getGlobalID() {
		return globalID;
	}

	public int getLocalId() {
		return localId;
	}
	
	public Connector getConnector() {
		return connector;
	}
	
	public void setCurrentLobby(Lobby currentLobby) {
		this.currentLobby = currentLobby;
	}

}
