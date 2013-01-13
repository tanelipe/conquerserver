using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary;
using GameServer.Processors;
using GameServer.Database;
namespace GameServer
{
    class Program
    {
        private static PacketProcessor PacketProcessor;
        private static DatabaseManager Database;
        static void Main(string[] args)
        {
            GameServerSocket Server = new GameServerSocket() ;
            Server.OnClientConnected = OnClientConnect;
            Server.OnClientDisconnected = OnClientDisconnect;
            Server.OnClientReceived = OnClientReceive;
            Server.Initialize();

            Database = new DatabaseManager();

            PacketProcessor = new PacketProcessor(Database);

            while (true)
            {

            }
        }
        private static void OnClientConnect(WinsockClient Socket, object NullParam)
        {
            Socket.Wrapper = new GameClient(Socket);
        }
        private static void OnClientDisconnect(WinsockClient Socket, object NullParam)
        {
            GameClient Client = Socket.Wrapper as GameClient;
        }
        private static unsafe void OnClientReceive(WinsockClient Socket, byte[] Packet, int Length)
        {
            GameClient Client = Socket.Wrapper as GameClient;
            PacketProcessor.Process(Client, Packet);
        }
    }
}
