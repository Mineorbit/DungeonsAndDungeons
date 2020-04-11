package net.pack.client;

import java.io.IOException;
import java.io.InputStream;

public class LengthClientPacket extends ClientPacket {
	ClientPacket innerPacket;
	
	public LengthClientPacket(ClientPacket innerPacket) {
		this.innerPacket = innerPacket;
	}
	
	public static ClientPacket fromInputStream(InputStream in) throws IOException {
		byte[] rawLen = in.readNBytes(2);
		int length = 0;
		
		length |= rawLen[0];
		length |= (int) rawLen[1] << 8;
		
		return new LengthClientPacket(ActionClientPacket.fromBytes(in.readNBytes(length)));
	}
}
