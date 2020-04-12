package net.pack;

public class LengthPacket implements Packet {
	private Packet innerPacket;
	
	public LengthPacket(Packet innerPacket) {
		this.innerPacket = innerPacket;
	}
	
	@Override
	public byte[] toBytes() {
		byte[] inner = innerPacket.toBytes();
		byte[] result = new byte[inner.length + 2];
		
		result[0] = (byte) (inner.length >> 8);
		result[1] = (byte) inner.length;
		
		for (int i = 0; i < inner.length; i++) {
			result[i + 2] = inner[i];
		}
		
		return result;
	}
}
