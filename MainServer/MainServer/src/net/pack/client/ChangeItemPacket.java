package net.pack.client;

public class ChangeItemPacket extends ActionClientPacket {

	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleChangeItem(this, arg);
	}
}
