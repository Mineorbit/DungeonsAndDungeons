package net;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class Connector {
	public Socket socket;
	public InputStream inStream;
	public OutputStream outStream;

	public void Send(byte[] data) {
		try {
			outStream.write(data);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		try {
			outStream.flush();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

	public byte[] Receive(int amount) {
		byte[] data = new byte[amount];
		for (int i = 0; i < amount; i++) {
			try {
				data[i] = inStream.readNBytes(1)[0];
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		return data;
	}

}
