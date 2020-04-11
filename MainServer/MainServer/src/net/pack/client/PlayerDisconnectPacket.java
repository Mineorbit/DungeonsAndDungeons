package net.pack.client;

public class PlayerDisconnectPacket extends ClientPacket {
	public PlayerDisconnectPacket() {}
	
	public static PlayerDisconnectPacket fromBytes(byte[] bytes) {
		return new PlayerDisconnectPacket();
	}
}
