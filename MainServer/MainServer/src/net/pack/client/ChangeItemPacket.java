package net.pack.client;

public class ChangeItemPacket extends ActionClientPacket {
	private byte item;
	
	public ChangeItemPacket(byte item) {
		this.item = item;
	}
	
	public static ChangeItemPacket fromBytes(byte[] bytes) {
		return new ChangeItemPacket(bytes[0]);
	}
	
	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleChangeItem(this, arg);
	}
	public byte getItem() {
		return item;
	}
}
