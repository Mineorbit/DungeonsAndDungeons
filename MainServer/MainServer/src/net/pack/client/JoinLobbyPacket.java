package net.pack.client;

import util.Util;

public class JoinLobbyPacket extends ActionClientPacket {
	private int lobbyId;
	
	public JoinLobbyPacket(int lobbyId) {
		this.lobbyId = lobbyId;
	}
	
	public static JoinLobbyPacket fromBytes(byte[] bytes) {
		int lobbyId = Util.bytesToInt(bytes);
		return new JoinLobbyPacket(lobbyId);
	}
	
	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleJoinLobby(this, arg);
	}

	public int getLobbyId() {
		return lobbyId;
	}
}
