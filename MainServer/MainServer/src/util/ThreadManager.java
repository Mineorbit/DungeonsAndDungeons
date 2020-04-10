package util;

import java.util.List;

public class ThreadManager {
public static ThreadManager instance;
public static List<Action> actions;
public static List<Action> localactions;


public void Update()
{
	if(!actions.isEmpty())
	{
	synchronized(this)
	{
		localactions=actions;
		actions.clear();
	}
	for(Action a : localactions)
	{
		a.work();
	}
	
	}
}
}
