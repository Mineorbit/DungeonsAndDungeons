package net.pack.client;

import java.nio.charset.StandardCharsets;

import util.Util;

public class PlayerConnectPacket extends ClientPacket {
	private String playerName;
	
	public PlayerConnectPacket(String playerName) {
		this.playerName = playerName;
	}
	
	public static PlayerConnectPacket fromBytes(byte[] bytes) {
		System.out.println("Create Name"+bytes.length);
		short nameLength = (short)((int)bytes[0]*256+(int)bytes[1]);
		String name = new String(Util.subArray(bytes, 2, 1+nameLength));
		return new PlayerConnectPacket(name);
	}

	public String getPlayerName() {
		return playerName;
	}
}
