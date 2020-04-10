package main;

import java.io.IOException;

public class Main {
	public Main() throws IOException {
		Server s = new Server(13565, 5);
		s.start();
	}

	public static void main(String[] args) {
		try {
			new Main();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
