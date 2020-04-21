package net.pack;

import util.Util;

public class InvitationPacket implements Packet {
	private int lobbyId;
	private int invitationId;
	public static byte id = PacketType.INVITATION.id;
	
	public InvitationPacket(int lobbyId, int invitationId) {
		this.lobbyId = lobbyId;
		this.invitationId = invitationId;
	}
	
	@Override
	public byte[] toBytes() {
		final byte[] lobbyIdBytes = Util.intToBytes(lobbyId);
		final byte[] invitationIdBytes = Util.intToBytes(invitationId);
		byte[] result = new byte[1 + 4 + 4];
		
		result[0] = id;
		for (int i = 0; i < 4; i++) {
			result[i + 1] = lobbyIdBytes[i];
		}
		for (int i = 0; i < 4; i++) {
			result[i + 5] = invitationIdBytes[i];
		}
		
		return result;
	}

}
