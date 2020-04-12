package net.pack.client;

public class PlayerDisconnectPacket extends ActionClientPacket {
	public PlayerDisconnectPacket() {}
	
	public static PlayerDisconnectPacket fromBytes(byte[] bytes) {
		return new PlayerDisconnectPacket();
	}

	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handlePlayerDisconnect(this, arg);
	}
}
