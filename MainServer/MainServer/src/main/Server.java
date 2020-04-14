package main;

import java.io.IOException;
import java.net.ServerSocket;
import java.util.HashMap;
import java.util.Map;

import logic.Lobby;
import logic.Player;
import net.ClientListener;

public class Server {
	private static Server instance = null;
	private int port;
	private int maxPlayers;
	private int freePlayerId = 0;
	private int freeLobbyId = 0;
	private ServerSocket serverSocket;
	private Map<Integer, Lobby> lobbies;
	private Map<Integer, Player> playersbyGlobalID;

	private Server(int p, int mP) {
		lobbies = new HashMap<Integer, Lobby>();
		playersbyGlobalID = new HashMap<Integer, Player>();
		port = p;
		maxPlayers = mP;
	}

	public static void initInstance(int p, int mP) {
		if (instance == null) {
			instance = new Server(p, mP);
		}
	}
	
	public static Server getInstance() {
		return Server.instance;
	}
	
	public int getMaxPlayers() {
		return maxPlayers;
	}

	public void setMaxPlayers(int maxPlayers) {
		this.maxPlayers = maxPlayers;
	}

	public int getFreePlayerId() {
		return freePlayerId;
	}

	public void setFreePlayerId(int freeId) {
		this.freePlayerId = freeId;
	}

	public Map<Integer, Lobby> getLobbies() {
		return lobbies;
	}

	public Map<Integer, Player> getPlayersbyGlobalID() {
		return playersbyGlobalID;
	}

	public void start() throws IOException {
		System.out.println("MainServer gestartet");
		serverSocket = new ServerSocket(port);
		ClientListener clientListener = new ClientListener(serverSocket);
		Thread listenThread = new Thread(clientListener);
		listenThread.start();
		System.out.println("Socket geöffnet auf " + port + ", Server wird betriebsbereit geführt");

	}

	public int getFreeLobbyId() {
		return freeLobbyId;
	}

	public void setFreeLobbyId(int freeLobbyId) {
		this.freeLobbyId = freeLobbyId;
	}

	class ServerControl implements Runnable {
		@Override
		public void run() {
			for (Player p : playersbyGlobalID.values()) {
				if (!p.setup) {
					p.setup = true;
					Thread t = new Thread(p.playerHandle);
					t.start();
				}
			}
		}
	}
}
