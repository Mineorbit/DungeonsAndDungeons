package main;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

import logic.Lobby;
import logic.Player;
import net.ClientHandler;
import util.ThreadManager;

public class Server {
	public static int  port;
	public static int  maxPlayers;
	public static  int playerCount;
	ServerSocket serverSocket;
	public List<Lobby> lobbies;
	public List<Player> players;
 public Server(int p,int mP)
 {
	 ThreadManager.instance = new ThreadManager();
	 lobbies = new ArrayList<Lobby>();
	 players = new ArrayList<Player>();
	 port = p;
	 maxPlayers  = mP;
	 
	 
 }
 public void start() throws IOException {
	 System.out.println("MainServer gestartet");
	 serverSocket = new ServerSocket(port);
	 accept();
 }
 
 public void accept() throws IOException
 {
	 Socket s = serverSocket.accept();
	 ClientHandler c = new ClientHandler(s);
	 Thread t = new Thread(c);
	 t.start();
	 accept();
 }
 
 
}
