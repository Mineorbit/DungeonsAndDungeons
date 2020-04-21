package net.pack;

public enum PacketType {
	CONNECTION_INFO((byte) 1),
	NOTIFICATION((byte) 2),
	INVITATION((byte) 3);
	
	public byte id;
	
	private PacketType(byte id) {
		this.id = id;
	}
}
