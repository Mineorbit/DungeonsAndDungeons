package net.pack;

import java.nio.charset.StandardCharsets;

public class NotificationPacket implements Packet {
	private String message;
	
	public NotificationPacket(String message) {
		this.message = message;
	}
	
	@Override
	public byte[] toBytes() {
		return message.getBytes(StandardCharsets.UTF_8);
	}
}
