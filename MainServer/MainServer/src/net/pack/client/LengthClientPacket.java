package net.pack.client;

import java.io.IOException;
import java.io.InputStream;

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
		int length = 0;
		
		length |= rawLen[0];
		length |= (int) rawLen[1] << 8;
		
		return new LengthClientPacket(ActionClientPacket.fromBytes(in.readNBytes(length)));
	}
}
