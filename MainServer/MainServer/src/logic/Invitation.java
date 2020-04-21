package logic;

import java.util.Calendar;
import java.util.Date;

public class Invitation {
	private int id;
	private Player recipient;
	private Lobby lobby;
	private Date expiration;
	
	public Invitation(int id, Player recipient, Lobby lobby) {
		this.id = id;
		this.recipient = recipient;
		this.lobby = lobby;
		
		// Add expiration date
		Calendar cal = Calendar.getInstance();
		cal.setTime(new Date());
		cal.add(Calendar.HOUR, 1);
		this.expiration = cal.getTime();
	}

	public Player getRecipient() {
		return recipient;
	}

	public Lobby getLobby() {
		return lobby;
	}

	public boolean isValid() {
		Date now = new Date();
		return now.before(expiration);
	}

	public int getId() {
		return id;
	}
}
