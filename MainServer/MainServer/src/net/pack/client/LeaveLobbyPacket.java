package net.pack.client;

public class LeaveLobbyPacket extends ActionClientPacket {
	public LeaveLobbyPacket() {}
	
	public static LeaveLobbyPacket fromBytes(byte[] bytes) {
		return new LeaveLobbyPacket();
	}
	
	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleLeaveLobby(this, arg);
	}
}
