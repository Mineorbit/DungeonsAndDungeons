package net.pack.client;

import java.util.Arrays;

import util.Util;

public class JoinLobbyPacket extends ActionClientPacket {
	private int lobbyId;
	private int invitationId;
	
	public JoinLobbyPacket(int lobbyId, int invitationId) {
		this.lobbyId = lobbyId;
		this.invitationId = invitationId;
	}
	
	public static JoinLobbyPacket fromBytes(byte[] bytes) {
		int lobbyId = Util.bytesToInt(bytes);
		int invitationId = Util.bytesToInt(Arrays.copyOfRange(bytes, 4, bytes.length));
		
		return new JoinLobbyPacket(lobbyId, invitationId);
	}
	
	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleJoinLobby(this, arg);
	}

	public int getLobbyId() {
		return lobbyId;
	}

	public int getInvitationId() {
		return invitationId;
	}
}
