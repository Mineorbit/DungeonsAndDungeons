package main;

import java.io.IOException;

public class Main {
	public static void main(String[] args) {
		try {
			Server s = new Server(13565, 5);
			s.start();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
