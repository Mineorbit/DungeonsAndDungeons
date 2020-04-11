package net.pack;

import java.util.Arrays;

public class LengthPacket implements Packet {
	private Packet innerPacket;
	
	public LengthPacket(Packet innerPacket) {
		this.innerPacket = innerPacket;
	}
	
	@Override
	public byte[] toBytes() {
		byte[] inner = innerPacket.toBytes();
		byte[] result = Arrays.copyOf(inner, inner.length + 2);
		
		result[inner.length] = (byte) (inner.length >> 1);
		result[inner.length + 1] = (byte) inner.length;
		
		return result;
	}
}
