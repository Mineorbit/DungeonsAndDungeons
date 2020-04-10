package net.pack;

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
	
}
