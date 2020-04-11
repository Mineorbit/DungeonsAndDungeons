package net.pack.client;

import java.nio.charset.StandardCharsets;

public class ChangeNamePacket extends ClientPacket {
	String newName;
	
	public ChangeNamePacket(String newName) {
		this.newName = newName;
	}
	
	public static PlayerConnectPacket fromBytes(byte[] bytes) {
		return new PlayerConnectPacket(new String(bytes, StandardCharsets.UTF_8));
	}
}
