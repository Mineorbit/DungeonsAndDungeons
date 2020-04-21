package logic;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import main.Server;

public class Lobby {
	private Map<Integer, Player> playersByLocalId;
	private Map<Integer, Invitation> invitations;
	
	private int freeInvitationId;
	private int lobbyId;
	// LocalId of MasterPlayer
	private int masterId;
	
	private long currentLevelId;
	
	public Lobby(int lobbyId, Player master) {
		master.setCurrentLobby(this);
		
		this.lobbyId = lobbyId;
		this.playersByLocalId = new HashMap<Integer, Player>();
		this.invitations = new HashMap<Integer, Invitation>();
		this.freeInvitationId = 0;
		this.masterId = 0;
		this.playersByLocalId.put(this.masterId, master);
	}
	
	public void invitePlayer(Player p) {
		// Create Invitation
		Invitation inv = new Invitation(freeInvitationId, p, this);
		invitations.put(freeInvitationId++, inv);
		
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

		removePlayer(playerToBan.getLocalId());
	}

	public void changeMaster(Player requester, int localId) {
		if (requester.getLocalId() == masterId) {
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
