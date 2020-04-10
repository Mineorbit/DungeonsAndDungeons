package logic;

import net.Connector;
import net.pack.Packet;

public class Player {

	public String name;
	public int globalid;
	public Connector connector;
	public PlayerHandle playerHandle;
	public boolean setup;

	public Player(String n, Connector info) {
		name = n;
		connector = info;
	}

	public void disconnect() {
		playerHandle.Disconnect();
	}

	public void Update(Packet p) {
		playerHandle.Update(p);
	}

}
