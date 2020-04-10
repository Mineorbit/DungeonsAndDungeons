package util;

import java.util.Arrays;

public class Util {
	public static byte[] toBytes(int x)
	{
	byte[] r = new byte[4];
	for(int i=0;i<4;i++)
	{
		r[i] = (byte)( x<<(i*8));
	}
	return r;
	}
	public static<T> byte[] subArray(byte[] data, int beg, int end) {
		return Arrays.copyOfRange(data, beg, end + 1);
	}

	
}
