package main;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

import logic.Lobby;
import logic.Player;
import net.ClientHandler;
import net.ClientListener;

public class Server {
	public static Server current;
	public static int  port;
	public static int  maxPlayers;
	public static int playerCount;
	public static int freeId = 0;
	ServerSocket serverSocket;
	public static List<Lobby> lobbies;
	public static List<Player> players;
 public Server(int p,int mP)
 {
	 current = this;
	 lobbies = new ArrayList<Lobby>();
	 players = new ArrayList<Player>();
	 port = p;
	 maxPlayers  = mP;
 }

 public void start() throws IOException {
	 System.out.println("MainServer gestartet");
	 serverSocket = new ServerSocket(port);
	 ClientListener clientListener = new ClientListener(serverSocket);
	 Thread listenThread = new Thread(clientListener);
	 listenThread.start();
	 System.out.println("Socket geöffnet auf "+port+", Server wird betriebsbereit geführt");
	 
 }
 	class ServerControl implements Runnable{

		@Override
		public void run() {
			for(Player p : players)
			{
				if(!p.setup)
				{
					p.setup = true;;
					Thread t = new Thread(p.playerHandle);
					t.start();
				}
			}
			
		}
 		
 	}
 
 
 
}
