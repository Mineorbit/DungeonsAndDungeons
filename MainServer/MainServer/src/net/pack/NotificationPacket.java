package net.pack;

import java.nio.charset.StandardCharsets;

import util.Util;

public class NotificationPacket implements Packet {
	private static byte id = PacketType.NOTIFICATION.id;
	private String message;
	
	public NotificationPacket(String message) {
		this.message = message;
	}
	
	@Override
	public byte[] toBytes() {
		final byte[] bytes = Util.stringToBytes(message);
		byte[] result = new byte[bytes.length + 1];
		
		result[0] = id;
		
		for (int i = 0; i < bytes.length; i++) {
			result[i + 1] = bytes[i];
		}
		
		return result;

	}
}
