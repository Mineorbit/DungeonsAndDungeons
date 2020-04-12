package net.pack.client;

public class ChangeColorPacket extends ActionClientPacket {

	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleChangeColor(this, arg);
	}
}
