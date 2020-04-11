package net.pack.client;

public enum ClientPacketType {
	PLAYER_CONNECT((byte) 1),
	PLAYER_DISCONNECT((byte) 2),
	LEAVE_LOBBY((byte) 3),
	JOIN_LOBBY((byte) 4),
	CHANGE_NAME((byte) 5),
	CHANGE_ITEM((byte) 6),
	CHANGE_COLOR((byte) 7);
	
	public final byte id;
	
	ClientPacketType(byte id) {
		this.id = id;
	}
}
