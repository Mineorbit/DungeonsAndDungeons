package logic;

import net.pack.client.ChangeColorPacket;
import net.pack.client.ChangeItemPacket;
import net.pack.client.ChangeNamePacket;
import net.pack.client.ClientPacketHandler;
import net.pack.client.JoinLobbyPacket;
import net.pack.client.LeaveLobbyPacket;
import net.pack.client.PlayerConnectPacket;
import net.pack.client.PlayerDisconnectPacket;

public class PlayerPacketHandler implements ClientPacketHandler<Player, Void> {
	private static PlayerPacketHandler instance = null;
	
	public PlayerPacketHandler() {}
	
	public static PlayerPacketHandler getInstance() {
		if (instance == null) {
			instance = new PlayerPacketHandler();
		}
		return instance;
	}
	
	@Override
	public Void handlePlayerConnect(PlayerConnectPacket p, Player arg) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Void handlePlayerDisconnect(PlayerDisconnectPacket p, Player arg) {
		arg.disconnect();
		return null;
	}

	@Override
	public Void handleLeaveLobby(LeaveLobbyPacket p, Player arg) {
		arg.leaveLobby();
		return null;
	}

	@Override
	public Void handleJoinLobby(JoinLobbyPacket p, Player arg) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Void handleChangeName(ChangeNamePacket p, Player arg) {
		arg.setName(p.getNewName());
		return null;
	}

	@Override
	public Void handleChangeItem(ChangeItemPacket p, Player arg) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Void handleChangeColor(ChangeColorPacket p, Player arg) {
		arg.setPlayerColor(p.getNewColor());
		return null;
	}

}
