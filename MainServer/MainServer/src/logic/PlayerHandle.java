package logic;

import java.io.IOException;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;

import main.Server;
import net.pack.Packet;

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
		Server.current.playerCount--;
		Server.current.players.remove(p);
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
		Queue<Packet> receivedPackets;

		public InputHandle(Player p) {
			receivedPackets = new LinkedList<Packet>();
			player = p;
		}

		@Override
		public void run() {
			while (p.playerHandle.Running) {
				byte[] data = player.connector.Receive(32);
				Packet result = Packet.FromData(data);
				receivedPackets.add(result);
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
						player.connector.Send(send.ToData());
					}
				}
			}
		}

	}

}
