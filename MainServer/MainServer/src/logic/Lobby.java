package logic;

import java.util.HashMap;
import java.util.Map;

import main.Server;

public class Lobby {
	public Map<Integer, Player> playersByLocalId;
	public int lobbyId;
	// LocalId of MasterPlayer
	public int masterId;

	public Lobby() {
		playersByLocalId = new HashMap<Integer, Player>();
	}

	public void invitePlayer(int playerId) {
		// Send Invite Packet to player (includes lobbyId)
	}

	public void invitePlayer(String username) {

	}

	public void kick(int localId) {

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

	public void ban(int globalId) {
		Player playerToBan = Server.getInstance()
				.getPlayersbyGlobalID().get(globalId);

		kick(playerToBan.localId);
	}

	public void startGame(String ip, int port) {
		// Send relay information to every player in lobby
	}
}
