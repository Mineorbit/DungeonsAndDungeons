package util;

import java.nio.charset.StandardCharsets;
import java.util.Arrays;

public class Util {
	public static byte[] intToBytes(int x) {
		byte[] r = new byte[4];
		for (int i = 0; i < 4; i++) {
			r[3 - i] = (byte) (x << (i * 8));
		}
		return r;
	}

	public static int bytesToInt(byte[] bytes) {
		int result = 0;
		for (int i = 0; i < 4; i++) {
			result |= ((int) bytes[3 - i]) >> (i * 8);
		}
		return result;
	}

	public static int readTwoBytes(byte a, byte b) {
		return (int) a << 8 | (int) b;
	}

	public static byte[] writeTwoBytes(int x) {
		return new byte[] { (byte) (x >> 8), (byte) x };
	}

	public static byte[] subArray(byte[] data, int beg, int end) {
		return Arrays.copyOfRange(data, beg, end + 1);
	}

	public static byte[] stringToBytes(String str) {
		final byte[] raw = str.getBytes(StandardCharsets.UTF_8);
		byte[] result = new byte[raw.length + 2];
		byte[] len = writeTwoBytes(raw.length);
		result[0] = len[0];
		result[1] = len[1];

		for (int i = 0; i < raw.length; i++) {
			result[i + 2] = raw[i];
		}

		return result;
	}

	public static String bytesToString(byte[] bytes) {
		int length = readTwoBytes(bytes[0], bytes[1]);

		return new String(Arrays.copyOfRange(bytes, 2, 2 + length), StandardCharsets.UTF_8);
	}
}
