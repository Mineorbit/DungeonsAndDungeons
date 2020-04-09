package net;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class ClientHandler implements Runnable {
Socket socket;
InputStream inStream;
OutputStream outStream;
enum State {Join,Add,Lobby,Remove,Start};
public ClientHandler(Socket s)
{
	socket = s;
}
@Override
public void run() {
System.out.println("Neue Verbindung: "+socket);
try {
	inStream = new BufferedInputStream(socket.getInputStream());
} catch (IOException e1) {
	// TODO Auto-generated catch block
	e1.printStackTrace();
}
try {
	outStream = new BufferedOutputStream(socket.getOutputStream());
} catch (IOException e) {
	// TODO Auto-generated catch block
	e.printStackTrace();
}

try {
	process();
} catch (IOException e) {
	// TODO Auto-generated catch block
	e.printStackTrace();
}

}
public void process() throws IOException
{
	Package p = receive();
	send(p);
}
public Package receive() throws IOException
{
	byte[] data = new byte[100];
	inStream.read(data,0,100);
	Package p = Package.Get(data);
	return null;
}

public void send(Package p) throws IOException
{	
	outStream.write(p.data);
	outStream.flush();
}
}
