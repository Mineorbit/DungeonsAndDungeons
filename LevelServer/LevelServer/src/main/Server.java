package main;

import java.net.InetSocketAddress;

import com.sun.net.httpserver.HttpServer;

public class Server {
HttpServer server;
public Server()
{
	server = new HttpServer.create(new InetSocketAddress("localhost",8585),0);
}
}
