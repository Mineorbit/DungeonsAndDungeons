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
	private PlayerHandle playerHandle;

	private Lobby currentLobby;

	// ID in lobby(temporary) also important for gameplay range: 0-3
	private int localId;

	private PlayerColor color;
	private byte item;

	public Player(int globalId, String n, Connector connector) {
		this.globalID = globalId;
		this.name = n;
		this.currentLobby = null;
		this.connector = connector;
		this.item = 0;
		
		this.playerHandle = new PlayerHandle(this);
		Thread handleThread = new Thread(this.playerHandle);
		handleThread.start();
	}

	public void joinLobby(int lobbyId, int invitationId) {
		Lobby l;
		Server server = Server.getInstance();
		synchronized (server) {
			l = server.getLobbies().get(lobbyId);
		}
		
		l.join(this, invitationId);
	}
	
	public void leaveLobby() {
		// Remove from current lobby
		synchronized (currentLobby) {
			currentLobby.removePlayer(localId);
		}
		
		// Create a new lobby for this player alone
		Server server = Server.getInstance();
		
		int lobbyId = -1;
		synchronized (server) {
			lobbyId = server.getFreeLobbyId();
		}
		
		Lobby l = new Lobby(lobbyId, this);
		synchronized (server) {
			server.getLobbies().put(lobbyId, l);
		}
		
		currentLobby = l;
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
		return color;
	}

	public void setPlayerColor(PlayerColor playerColor) {
		this.color = playerColor;
	}

	public int getGlobalID() {
		return globalID;
	}

	public int getLocalId() {
		return localId;
	}
	
	public void setLocalId(int localId) {
		this.localId = localId;
	}
	
	public Connector getConnector() {
		return connector;
	}
	
	public void setCurrentLobby(Lobby currentLobby) {
		this.currentLobby = currentLobby;
	}

	public byte getItem() {
		return item;
	}

	public void setItem(byte item) {
		this.item = item;
	}

}
