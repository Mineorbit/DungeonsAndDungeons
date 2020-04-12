package util;

import java.nio.charset.StandardCharsets;
import java.util.Arrays;

public class Util {
	public static byte[] toBytes(int x) {
		byte[] r = new byte[4];
		for (int i = 0; i < 4; i++) {
			r[3-i] = (byte) (x << (i * 8));
		}
		return r;
	}

	public static byte[] subArray(byte[] data, int beg, int end) {
		return Arrays.copyOfRange(data, beg, end + 1);
	}

	public static byte[] stringToBytes(String str) {
		final byte[] raw = str.getBytes(StandardCharsets.UTF_8);
		byte[] result = new byte[raw.length + 2];
		
		for (int i = 0; i < raw.length; i++) {
			result[i + 2] = raw[i];
		}
		
		return result;
	}
	
	public static String bytesToString(byte[] bytes) {
		int length = 0;
		
		length |= bytes[0];
		length |= (int) bytes[1] << 8;
		
		return new String(Arrays.copyOfRange(bytes, 2, 2 +  length), StandardCharsets.UTF_8);
	}
}
