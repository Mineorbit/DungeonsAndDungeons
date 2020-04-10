package net.pack;

public class Packet {
	enum Target {
		Lobby, Friends, All
	};

	public Target target;

	public static Packet FromData(byte[] data) {
		return new Packet();
	}

	public byte[] ToData() {
		return new byte[32];
	}
}
