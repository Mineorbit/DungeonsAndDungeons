package logic;

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
		this.lobbyId = lobbyId;
		this.playersByLocalId = new HashMap<Integer, Player>();
		this.invitations = new HashMap<Integer, Invitation>();
		this.freeInvitationId = 0;
		
		// Add the master
		this.masterId = 0;
		this.playersByLocalId.put(this.masterId, master);
		master.setCurrentLobby(this);
	}
	
	public void invitePlayer(Player p) {
		// Create Invitation
		Invitation inv = new Invitation(freeInvitationId, p, this);
		invitations.put(freeInvitationId++, inv);
		
		// Send Invitation Packet to player (includes lobbyId)
	}

	public void invitePlayer(String username) {

	}

	private int getFreeLocalId() {
		for (int i = 0; i < 4; i++) {
			if (!playersByLocalId.containsKey(i)) return i;
		}
		return -1;
	}
	
	public void join(Player p, int invitationId) {
		// Check if this Player has a valid invitation
		if (invitations.containsKey(invitationId)) {
			Invitation inv = invitations.get(invitationId);
			if (inv.isValid()) {
				inv.use();
				
				// Add this player
				int localId = getFreeLocalId();
				playersByLocalId.put(localId, p);
				
				p.setLocalId(localId);
				p.setCurrentLobby(this);
			} else {
				p.sendNotification("This invitation is not valid");
			}
		} else {
			p.sendNotification("You were not invited to this lobby");
		}
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
