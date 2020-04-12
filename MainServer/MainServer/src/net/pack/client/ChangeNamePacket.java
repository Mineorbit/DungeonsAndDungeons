package net.pack.client;

import util.Util;

public class ChangeNamePacket extends ClientPacket {
	String newName;
	
	public ChangeNamePacket(String newName) {
		this.newName = newName;
	}
	
	public static PlayerConnectPacket fromBytes(byte[] bytes) {
		return new PlayerConnectPacket(Util.bytesToString(bytes));
	}
}
