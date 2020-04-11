package net.pack;

public class ConnectionInfoPacket implements Packet {
	private int globalId;
	
	public ConnectionInfoPacket(int globalId) {
		this.globalId = globalId;
	}

	@Override
	public byte[] toBytes() {
		return util.Util.toBytes(globalId);
	}
}
