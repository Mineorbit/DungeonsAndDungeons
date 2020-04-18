package logic;

import java.util.List;

public class GameServer {
public String ip;
enum Status {open, occupied};
Status currentStatus;
float upTime;
float qualityRating = 0; //from 0 to 1 measured with dropped packages, necessary conflict resolutions etc over upTime
float latency = 0; //measured in ms


//Tells server, if open, to prepare for these players playing on this map
public void SendStartRequest(long levelId,List<Player> players)
{
	
}

}
