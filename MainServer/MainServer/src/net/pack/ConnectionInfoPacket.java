package net.pack;

public class ConnectionInfoPacket implements Packet {
	private int globalId;
	private static byte id = PacketType.CONNECTION_INFO.id;
	
	public ConnectionInfoPacket(int globalId) {
		this.globalId = globalId;
	}

	@Override
	public byte[] toBytes() {
		final byte[] bytes = util.Util.intToBytes(globalId);
		byte[] result = new byte[bytes.length + 1];
		
		System.out.println("ID: "+globalId);
		
		result[0] = id;
		
		for (int i = 0; i < bytes.length; i++) {
			result[i + 1] = bytes[i];
		}
		
		return result;
	}
}
