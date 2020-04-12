package net.pack.client;

public class JoinLobbyPacket extends ActionClientPacket {

	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleJoinLobby(this, arg);
	}
}
