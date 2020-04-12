package net.pack.client;

import util.Util;

public class ChangeNamePacket extends ActionClientPacket {
	private String newName;
	
	public ChangeNamePacket(String newName) {
		this.newName = newName;
	}
	
	public static PlayerConnectPacket fromBytes(byte[] bytes) {
		return new PlayerConnectPacket(Util.bytesToString(bytes));
	}

	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleChangeName(this, arg);
	}

	public String getNewName() {
		return newName;
	}
}
