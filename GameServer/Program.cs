using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary;
namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServerSocket Server = new GameServerSocket();
            Server.OnClientConnected = OnClientConnect;
            Server.OnClientDisconnected = OnClientDisconnect;
            Server.OnClientReceived = OnClientReceive;
            Server.Initialize();

            while (true)
            {

            }
        }
        private static void OnClientConnect(WinsockClient Socket, object NullParam)
        {

        }
        private static void OnClientDisconnect(WinsockClient Socket, object NullParam)
        {

        }
        private static unsafe void OnClientReceive(WinsockClient Socket, byte[] Packet, int Length)
        {

        }
    }
}
