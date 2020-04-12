package net.pack.client;

public interface ClientPacketHandler<T, R> {
	public R handlePlayerConnect(PlayerConnectPacket p, T arg);
	public R handlePlayerDisconnect(PlayerDisconnectPacket p, T arg);
	public R handleLeaveLobby(LeaveLobbyPacket p, T arg);
	public R handleJoinLobby(JoinLobbyPacket p, T arg);
	public R handleChangeName(ChangeNamePacket p, T arg);
	public R handleChangeItem(ChangeItemPacket p, T arg);
	public R handleChangeColor(ChangeColorPacket p, T arg);
}
