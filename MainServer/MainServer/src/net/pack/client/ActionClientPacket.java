package net.pack.client;

import java.util.Arrays;

public class ActionClientPacket extends ClientPacket {
	public static ClientPacket fromBytes(byte[] bytes) {
		final byte id = bytes[0];
		final byte[] content = Arrays.copyOfRange(bytes, 2, bytes.length);
		
		if (id == ClientPacketType.PLAYER_CONNECT.id) {
			return PlayerConnectPacket.fromBytes(content);
		} else if (id == ClientPacketType.PLAYER_DISCONNECT.id) {
			return null;
		} else if (id == ClientPacketType.LEAVE_LOBBY.id) {
			return null;
		} else if (id == ClientPacketType.JOIN_LOBBY.id) {
			return null;
		} else if (id == ClientPacketType.LEAVE_LOBBY.id) {
			return null;
		} else if (id == ClientPacketType.CHANGE_NAME.id) {
			return null;
		} else if (id == ClientPacketType.CHANGE_ITEM.id) {
			return null;
		} else if (id == ClientPacketType.CHANGE_COLOR.id) {
			return null;
		} else {
			return null;
		}
	}
}
