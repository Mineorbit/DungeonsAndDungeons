package logic;

import java.io.IOException;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;

import main.Server;
import net.pack.Packet;
import net.pack.client.ClientPacket;

public class PlayerHandle implements Runnable {
	Player p;
	InputHandle iH;
	OutputHandle oH;
	boolean Running;

	@Override
	public void run() {
		iH = new InputHandle(p);
		oH = new OutputHandle(p);
		Thread iT = new Thread(iH);
		Thread oT = new Thread(oH);
		iT.start();
		oT.start();
		while (Running) {

		}
	}

	public void Update(Packet p) {
		oH.toSend.add(p);
	}

	public PlayerHandle(Player player) {
		Running = true;
		p = player;
	}

	public void Disconnect() {
		Server.getInstance();
		try {
			p.connector.socket.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		Running = false;

	}

	class InputHandle implements Runnable {
		Player player;
		Queue<ClientPacket> receivedPackets;

		public InputHandle(Player p) {
			receivedPackets = new LinkedList<ClientPacket>();
			player = p;
		}

		@Override
		public void run() {
			while (p.playerHandle.Running) {
				receivedPackets.add(ClientPacket.fromInputStream(player.connector.inStream));
			}
		}
	}

	class OutputHandle implements Runnable {
		Player player;
		public Queue<Packet> toSend;

		public OutputHandle(Player p) {
			toSend = new LinkedList<Packet>();
			player = p;
		}

		@Override
		public void run() {
			while (p.playerHandle.Running) {
				if (!toSend.isEmpty()) {
					Packet send = toSend.poll();
					if (send != null) {
						player.connector.Send(send.toBytes());
					}
				}
			}
		}
	}

}
