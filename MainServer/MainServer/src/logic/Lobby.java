package logic;

import java.util.HashMap;
import java.util.Map;

import main.Server;

public class Lobby {
	private Map<Integer, Player> playersByLocalId;
	private int lobbyId;
	// LocalId of MasterPlayer
	private int masterId;
	
	private long currentLevelId;
	
	public Lobby(int lobbyId, Player master) {
		this.lobbyId = lobbyId;
		this.playersByLocalId = new HashMap<Integer, Player>();
		this.masterId = 0;
		this.playersByLocalId.put(this.masterId, master);
	}

	public void invitePlayer(int playerId) {
		// Send Invite Packet to player (includes lobbyId)
	}

	public void invitePlayer(String username) {

	}

	public void removePlayer(int localId) {
		playersByLocalId.remove(localId);
		
		// If there are no more players in the lobby,
		// remove this lobby
		if (playersByLocalId.isEmpty()) {
			Server server = Server.getInstance();
			synchronized (server) {
				server.getLobbies().remove(lobbyId);
			}
		}
	}

	public void removePlayerByGlobalId(int globalId) {
		Player playerToBan = Server.getInstance()
				.getPlayersbyGlobalID().get(globalId);

		removePlayer(playerToBan.localId);
	}

	public void changeMaster(Player requester, int localId) {
		if (requester.localId == masterId) {
			masterId = localId;
			// Send Message to new Master
			requester.sendNotification("You are now the new master");
		} else {
			// Send Message to requester that operation was illegal
			requester.sendNotification("You aren't allowed to do that");
		}
	}

	public void startGame(String ip, int port) {
		// Send relay information to every player in lobby
	}
}
