package util;

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
	
}
