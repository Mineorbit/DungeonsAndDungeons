package net.pack.client;

import java.io.IOException;
import java.io.InputStream;

import util.Util;

public class LengthClientPacket extends ClientPacket {
	private ActionClientPacket innerPacket;
	
	public LengthClientPacket(ActionClientPacket innerPacket) {
		this.innerPacket = innerPacket;
	}
	
	public ActionClientPacket getInnerPacket() {
		return innerPacket;
	}
	
	public static LengthClientPacket fromInputStream(InputStream in) throws IOException {
		byte[] rawLen = in.readNBytes(2);
		int length = Util.readTwoBytes(rawLen[0], rawLen[1]);
				
		System.out.println("Length of new packet: " + length);
		
		return new LengthClientPacket(ActionClientPacket.fromBytes(in.readNBytes(length)));
	}
}
