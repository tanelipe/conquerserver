using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary;
namespace AuthenticationServer
{
    public class AuthClient
    {
        private AuthCryptography Cipher;
        private WinsockClient Socket;
        private uint AccountID;
      

        public AuthClient(WinsockClient Socket)
        {
            this.Socket = Socket;
            Cipher = new AuthCryptography();
            Cipher.Initialize();
        }

        public uint GetAccountID()
        {
            return AccountID;
        }
        public void SetAccountID(uint ID)
        {
            AccountID = ID;
        }

        public void Send(byte[] Packet)
        {
            Cipher.Encrypt(Packet);
            Socket.Send(Packet);
        }
        public unsafe void Send(void* pPacket, ushort Size)
        {
            byte* ptr = (byte*)pPacket;
            byte[] Packet = new byte[Size];

            
            for (int i = 0; i < Packet.Length; i++)
            {
                Packet[i] = ptr[i];
            }
            Send(Packet);
        }

        public void Decrypt(byte[] Packet)
        {
            Cipher.Decrypt(Packet);
        }
        public void Disconnect()
        {
            Socket.Disconnect();
        }
    }
}
