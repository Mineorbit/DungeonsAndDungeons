package net.pack;

import java.util.List;

import logic.Player;

public class Packet {
	List<Player> t;

	public Packet(List<Player> targets) {
		t = targets;
	}

	public static Packet FromData(byte[] data) {
		return new Packet(null);
	}

	public byte[] ToData() {
		return new byte[32];
	}
}
