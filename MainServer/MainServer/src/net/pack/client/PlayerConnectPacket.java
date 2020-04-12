package net.pack.client;

import util.Util;

public class PlayerConnectPacket extends ClientPacket {
	private String playerName;
	
	public PlayerConnectPacket(String playerName) {
		this.playerName = playerName;
	}
	
	public static PlayerConnectPacket fromBytes(byte[] bytes) {
		return new PlayerConnectPacket(Util.bytesToString(bytes));
	}

	public String getPlayerName() {
		return playerName;
	}
}
