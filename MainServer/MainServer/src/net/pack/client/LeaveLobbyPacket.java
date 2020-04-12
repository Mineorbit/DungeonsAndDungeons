package net.pack.client;

public class LeaveLobbyPacket extends ActionClientPacket {

	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleLeaveLobby(this, arg);
	}
}
