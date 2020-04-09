package main;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

import net.ClientHandler;

public class Server {
	public static int  port;
	public static int  maxPlayers;
	public static  int playerCount;
	ServerSocket serverSocket;
	public List<Thread> threads;
 public Server(int p,int mP)
 {
	 threads = new ArrayList<Thread>();
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
	 threads.add(t);
	 t.start();
	 accept();
 }
 
 
}
