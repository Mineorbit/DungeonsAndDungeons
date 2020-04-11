package logic;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import main.Server;

public class Lobby {
	public Map<Integer, Player> playersByLocalId;
	public int globalLobbyId;
	// LocalId of MasterPlayer
	public int masterId = 0;

	public Lobby() {
		playersByLocalId = new HashMap<Integer, Player>();
	}

	public void InvitePlayer(int playerId) {
		// Send Invite Packet to player (includes lobbyId)
	}

	public void InvitePlayer(String username) {

	}

	public void kick(int localId) {

	}

	public void changeMaster(Player requester, int localId) {
		if (requester.localId == masterId) {
			masterId = localId;
			// Send Message to new Master
		} else {
			// Send Message to requester that operation was illegal
		}
	}

	public void ban(int globalId) {
		Player playerToBan = Server.getInstance().getPlayersbyGlobalID().get(globalId);

		kick(playerToBan.localId);
	}

	public void startGame(String ip, int port) {
		// Send relay information to every player in lobby
	}
}
