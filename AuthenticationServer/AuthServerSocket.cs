using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetworkLibrary;
namespace AuthenticationServer
{
    public class AuthServerSocket : ServerSocket
    {
        public AuthServerSocket()
            : base(9958, 10, 8192)
        {

            base.OnClientConnected = OnClientConnect;
            base.OnClientDisconnected = OnClientDisconnect;
            base.OnClientReceived = OnClientReceive;
        }

        public override IPacketCipher CreatePacketCipher()
        {
            IPacketCipher cipher = new AuthServerCipher();
            cipher.Initialize();
            return cipher;
        }

        private void OnClientConnect(WinsockClient Socket, object NullParam)
        {
            
        }
        private void OnClientDisconnect(WinsockClient Socket, object NullParam)
        {
            
        }
        private void OnClientReceive(WinsockClient Socket, byte[] Packet, int Size)
        {
            
        }

    }
}
