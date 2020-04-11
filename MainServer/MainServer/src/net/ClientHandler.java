package net;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

import logic.Player;
import logic.PlayerHandle;
import main.Server;
import net.pack.ConnectionInfoPacket;
import net.pack.LengthPacket;
import net.pack.Packet;
import net.pack.client.ClientPacket;
import net.pack.client.LengthClientPacket;
import net.pack.client.PlayerConnectPacket;
import util.Util;

import java.util.*;

public class ClientHandler implements Runnable {
	Socket socket;
	InputStream inStream;
	OutputStream outStream;

	public ClientHandler(Socket s) {
		socket = s;
	}

	@Override
	public void run() {
		Server server = Server.getInstance();

		System.out.println("Neue Verbindung: " + socket);

		try {
			inStream = new BufferedInputStream(socket.getInputStream());
			outStream = new BufferedOutputStream(socket.getOutputStream());
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}

		Connector connector = new Connector();
		connector.socket = socket;
		connector.inStream = inStream;
		connector.outStream = outStream;

		int playerId = -1;

		// Handshake Server -> Client
		synchronized (server) {
			playerId = server.getFreeId();
			server.setFreeId(playerId + 1);
		}
		Packet toSend = new LengthPacket(new ConnectionInfoPacket(playerId));
		connector.Send(toSend.toBytes());

		// Handshake Client -> Server
		String playerName = "";
		ClientPacket recv;
		try {
			recv = ClientPacket.fromInputStream(connector.inStream);
		} catch (IOException e) {
			e.printStackTrace();
			return;
		}
		if (recv instanceof LengthClientPacket) {
			LengthClientPacket lengthPacket = (LengthClientPacket) recv;
			ClientPacket inner = lengthPacket.getInnerPacket();

			if (inner instanceof PlayerConnectPacket) {
				playerName = ((PlayerConnectPacket) inner).getPlayerName();
			} else {
				return;
			}
		} else {
			return;
		}

		// PlayerSetup
		Player p = new Player(playerName, connector);
		p.globalID = playerId;
		p.playerHandle = new PlayerHandle(p);

		synchronized (server) {
			server.getPlayersbyGlobalID().put(p.globalID, p);
		}

		System.out.println("Neuer Spieler: " + playerName);
		System.out.println(" Globale Id:" + p.globalID);
	}

}
