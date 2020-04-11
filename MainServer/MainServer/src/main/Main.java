package main;

import java.io.IOException;

public class Main {
	public static void main(String[] args) {
		try {
			Server.initInstance(13565, 5);
			Server.getInstance().start();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
