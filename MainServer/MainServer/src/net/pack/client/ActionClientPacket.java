package net.pack.client;

import java.util.Arrays;

public abstract class ActionClientPacket extends ClientPacket {
	public static ActionClientPacket fromBytes(byte[] bytes) {
		final byte id = bytes[0];
		final byte[] content = Arrays.copyOfRange(bytes, 1, bytes.length);
		
		if (id == ClientPacketType.PLAYER_CONNECT.id) {
			return PlayerConnectPacket.fromBytes(content);
		} else if (id == ClientPacketType.PLAYER_DISCONNECT.id) {
			return PlayerDisconnectPacket.fromBytes(content);
		} else if (id == ClientPacketType.LEAVE_LOBBY.id) {
			return LeaveLobbyPacket.fromBytes(bytes);
		} else if (id == ClientPacketType.JOIN_LOBBY.id) {
			return JoinLobbyPacket.fromBytes(bytes);
		} else if (id == ClientPacketType.CHANGE_NAME.id) {
			return ChangeNamePacket.fromBytes(bytes);
		} else if (id == ClientPacketType.CHANGE_ITEM.id) {
			return ChangeItemPacket.fromBytes(bytes);
		} else if (id == ClientPacketType.CHANGE_COLOR.id) {
			return ChangeColorPacket.fromBytes(bytes);
		} else {
			return null;
		}
	}
	
	public abstract <T, R> R handle(ClientPacketHandler<T, R> handler, T arg);
}
