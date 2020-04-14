package net.pack.client;

import logic.PlayerColor;

public class ChangeColorPacket extends ActionClientPacket {
	private PlayerColor newColor;

	public ChangeColorPacket(PlayerColor newColor) {
		this.newColor = newColor;
	}

	public static ChangeColorPacket fromBytes(byte[] bytes) {
		switch (bytes[0]) {
		case 0:
			return new ChangeColorPacket(PlayerColor.RED);
		case 1:
			return new ChangeColorPacket(PlayerColor.BLUE);
		case 2:
			return new ChangeColorPacket(PlayerColor.GREEN);
		case 3:
			return new ChangeColorPacket(PlayerColor.YELLOW);
		}
		return null;
	}

	@Override
	public <T, R> R handle(ClientPacketHandler<T, R> handler, T arg) {
		return handler.handleChangeColor(this, arg);
	}

	public PlayerColor getNewColor() {
		return newColor;
	}
}
