package logic;

import java.io.IOException;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;

import main.Server;
import net.pack.Packet;
import net.pack.client.ActionClientPacket;
import net.pack.client.ClientPacket;
import net.pack.client.LengthClientPacket;
import net.pack.client.PlayerConnectPacket;

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
			synchronized (iH) {
				if (!iH.receivedPackets.isEmpty()) {
					ClientPacket recv = iH.receivedPackets.poll();
					
					if (recv instanceof LengthClientPacket) {
						LengthClientPacket lengthPacket = (LengthClientPacket) recv;
						ActionClientPacket inner = lengthPacket.getInnerPacket();

						inner.handle(PlayerPacketHandler.getInstance(), p);
					} else {
						System.err.println("Recieved a non-length packet");
					}
				}
			}
		}
	}

	public void send(Packet p) {
		synchronized (oH) {
			oH.toSend.add(p);
		}
	}

	public PlayerHandle(Player player) {
		Running = true;
		p = player;
	}

	public void disconnect() {
		Running = false;
		try {
			p.getConnector().socket.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
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
				try {
					receivedPackets.add(ClientPacket.fromInputStream(player.getConnector().inStream));
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
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
						player.getConnector().Send(send.toBytes());
					}
				}
			}
		}
	}

}
