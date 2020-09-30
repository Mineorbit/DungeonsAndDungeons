using System;
namespace NetServer
{ 
public class Server
{
	PacketTypeHandler packetTypeHandler;

	int port = 13565;
	public Server()
    {
		packetTypeHandler = new PacketTypeHandler();

    }




}
}