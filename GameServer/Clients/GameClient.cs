using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary;
namespace GameServer
{
    public class GameClient
    {
        private WinsockClient Connection;
        private GameCryptography Crypto;

        public GameClient(WinsockClient Connection)
        {
            this.Connection = Connection;

            Crypto = new GameCryptography();
            Crypto.Initialize();
        }

        public void Send(byte[] Packet)
        {
            Crypto.Encrypt(Packet);
            Connection.Send(Packet);
        }
        public void Decrypt(byte[] Packet)
        {
            Crypto.Decrypt(Packet);
        }
        public void Disconnect()
        {
            Connection.Disconnect();
        }
    }
}
