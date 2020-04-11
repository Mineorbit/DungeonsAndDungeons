package net.pack.client;

import java.io.IOException;
import java.io.InputStream;

public abstract class ClientPacket {
	public static ClientPacket fromInputStream(InputStream in) throws IOException {
		return LengthClientPacket.fromInputStream(in);
	}
}
