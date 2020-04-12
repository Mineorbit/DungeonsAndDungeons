package net.pack.client;

import util.Util;

public class PlayerConnectPacket extends ActionClientPacket {
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

	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handlePlayerConnect(this, arg);
	}
}
