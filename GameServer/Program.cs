using System;
using System.Threading;
using NetworkLibrary;
using GameServer.Processors;
using GameServer.Database;
namespace GameServer
{
    class Program
    {
        private static ItemTypeLoader ItemTypeLoader;

        private static PacketProcessor PacketProcessor;
        private static DatabaseManager Database;

        static void Main(string[] args)
        {
            Console.Title = "ConquerServer - Game";

            GameServerSocket Server = new GameServerSocket() ;
            Server.OnClientConnected = OnClientConnect;
            Server.OnClientDisconnected = OnClientDisconnect;
            Server.OnClientReceived = OnClientReceive;
            Server.Initialize();

            Database = new DatabaseManager();
            PacketProcessor = new PacketProcessor(Database);

            ItemTypeLoader = new ItemTypeLoader();
            ItemTypeLoader.LoadItems();

            while (true)
            {
                Thread.Sleep(1);
            }
        }
        private static void OnClientConnect(WinsockClient Socket, object NullParam)
        {
            Socket.Wrapper = new GameClient(Socket);
        }
        private static void OnClientDisconnect(WinsockClient Socket, object NullParam)
        {
            GameClient Client = Socket.Wrapper as GameClient;
            Database.SaveCharacter(Client);
            EntityManager.Remove(Client);
        }
        private static unsafe void OnClientReceive(WinsockClient Socket, byte[] Packet, int Length)
        {
            GameClient Client = Socket.Wrapper as GameClient;
            Client.Decrypt(Packet);
            PacketProcessor.Process(Client, Packet);
        }
    }
}
