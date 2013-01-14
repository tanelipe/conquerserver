using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary;
namespace GameServer
{
    public unsafe class GameClient
    {
        private WinsockClient Connection;
        private GameCryptography Crypto;
        public Entity Entity;
        public uint UID;

        public GameClient(WinsockClient Connection)
        {
            this.Connection = Connection;
   
            Crypto = new GameCryptography();
            Crypto.Initialize();
            Entity = new Entity(this);
        }

        public void Send(void* Packet, ushort Size)
        {
            byte[] tmp = new byte[Size];
            fixed (byte* pPacket = tmp)
            {
                Memory.Copy(Packet, pPacket, Size);
            }
            Send(tmp);
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
        public void GenerateKeys(uint Key1, uint Key2)
        {
            Crypto.GenerateKeys(Key1, Key2);
        }
        public void Disconnect()
        {
            Connection.Disconnect();
        }
    }
}
