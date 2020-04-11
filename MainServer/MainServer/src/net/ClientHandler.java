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
		System.out.println("Neue Verbindung: " + socket);
		try {
			inStream = new BufferedInputStream(socket.getInputStream());
			outStream = new BufferedOutputStream(socket.getOutputStream());
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}

		Connector info = new Connector();
		info.socket = socket;
		info.inStream = inStream;
		info.outStream = outStream;

		String playerName = "";
//Handshake
		byte[] welcomeData = info.Receive(32);
		if (welcomeData[0] != 42) {
			System.out.println("Invalider Nutzer");
			return;
		}

		playerName = new String(Util.subArray(welcomeData, 1, 31));
//PlayerSetup
		Random r = new Random();
		if (playerName.equals("")) {
			playerName = "Testplayer" + r.nextInt();
		}
		Player p;
		synchronized (this) {
			p = new Player(playerName, info);
			p.globalid = Server.freeId;
			Server.freeId++;
			Server.playerCount++;
			p.playerHandle = new PlayerHandle(p);
			Server.current.playersbyGlobalID.put(p.globalid,p);
		}
		System.out.println("Neuer Spieler: " + playerName);
		System.out.println(" Globale Id:" + p.globalid);
	}

}
