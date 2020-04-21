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
	boolean running;

	@Override
	public void run() {
		iH = new InputHandle(this);
		oH = new OutputHandle(this);
		Thread iT = new Thread(iH);
		Thread oT = new Thread(oH);
		iT.start();
		oT.start();
		while (running) {
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
		running = true;
		p = player;
	}

	public void disconnect() {
		running = false;
		try {
			p.getConnector().socket.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	class InputHandle implements Runnable {
		PlayerHandle handle;
		Queue<ClientPacket> receivedPackets;

		public InputHandle(PlayerHandle handle) {
			receivedPackets = new LinkedList<ClientPacket>();
			this.handle = handle;
		}

		@Override
		public void run() {
			while (handle.running) {
				try {
					receivedPackets.add(ClientPacket.fromInputStream(handle.p.getConnector().inStream));
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
	}

	class OutputHandle implements Runnable {
		PlayerHandle handle;
		public Queue<Packet> toSend;

		public OutputHandle(PlayerHandle handle) {
			toSend = new LinkedList<Packet>();
			this.handle = handle;
		}

		@Override
		public void run() {
			while (handle.running) {
				if (!toSend.isEmpty()) {
					Packet send = toSend.poll();
					if (send != null) {
						handle.p.getConnector().Send(send.toBytes());
					}
				}
			}
		}
	}

}
