package net;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class ClientListener implements Runnable {

	ServerSocket serverSocket;

	public ClientListener(ServerSocket sS) {
		serverSocket = sS;
	}

	@Override
	public void run() {
		try {
			accept();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	public void accept() throws IOException {
		Socket s = serverSocket.accept();
		ClientHandler c = new ClientHandler(s);
		Thread t = new Thread(c);
		t.start();
		accept();
	}

}
