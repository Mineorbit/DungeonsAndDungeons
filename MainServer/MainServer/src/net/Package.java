package net;

public class Package {
	enum Type {Connect,JoinLobby,LeaveLobby, CreateLobby,PlayerJoined,PlayerLeft,ChangeClass,ChangeColor,StartGame};
	/*
	 * Package Layouts:
	 * 
	 * Connect: sign+username(31 chars)
	 * 
	 * Join Lobby: sign+LobbyId(4 bytes)
	 * Leave Lobby: sign+empty
	 * Create Lobby: sign+signPassword+Password(30 bytes)
	 * Player Joined: sign+signPassword+Password(30 bytes)
	 */
	

	public Type type;
	public Object[] content;
	public static Package Create(Type t, Object[] c) {
		Package p =  new Package();
		p.type = t;
		p.content = c;
		return p;
	}
	public static Package From(byte[] d) {
		// TODO Auto-generated method stub
		return null;
	}
	public static byte[] To(Package p) {
		byte[] data = new byte[32];
		data[0] = (byte) p.type.ordinal();
		
		return data;
	}
}
