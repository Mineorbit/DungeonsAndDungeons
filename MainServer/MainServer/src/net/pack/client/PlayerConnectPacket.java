package net.pack.client;

import java.nio.charset.StandardCharsets;

public class PlayerConnectPacket extends ClientPacket {
	String playerName;
	
	public PlayerConnectPacket(String playerName) {
		this.playerName = playerName;
	}
	
	public static PlayerConnectPacket fromBytes(byte[] bytes) {
		return new PlayerConnectPacket(new String(bytes, StandardCharsets.UTF_8));
	}
}
