package logic;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import main.Server;

public class Lobby {
	public Map<Integer,Player> playersByLocalId;
	public int id;
	//LocalId of MasterPlayer
	public int masterId = 0;

	public Lobby() {
		players = new ArrayList<Player>();
	}
	public void InvitePlayer(int playerId)
	{
		
	}
	public void InvitePlayer(String username)
	{
		
	}
	public void kick(int localId)
	{
		
	}
	public void changeMaster(Player requester,int localId)
	{
		if(requester.localId==masterId)
		{
			masterId = localId;
			//Send Message to new Master
		}else
		{
			//Send Message to requester that operation was illegal
		}
	}
	public void ban(int globalId)
	{
		Player playerToBan = Server.GetPlayer(globalId);
		
		kick(playerToBan.localId);
	}
	public void startGame(String ip, int port)
	{
		//Send relay information to every player in lobby
	}
}
