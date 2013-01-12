using System;

namespace NetworkLibrary
{
    using System.Net.Sockets;

    public class WinsockClient : IDisposable
    {
        private IPacketCipher Cipher;
        private ServerSocket Server;
        private Socket Connection;
        private byte[] Buffer;

        public WinsockClient(ServerSocket Server, Socket Connection, int BufferSize, IPacketCipher Cipher)
        {
            this.Server = Server;
            this.Connection = Connection;
            this.Cipher = Cipher;

            Buffer = new byte[BufferSize];
        }

        public void Send(byte[] Packet)
        {
            try
            {
                if (Cipher != null)
                    Cipher.Encrypt(Packet, Packet.Length);
                Connection.Send(Packet);
            }
            catch (Exception exception)
            {
#if DEBUG
                Console.WriteLine(exception.ToString());
#endif
            }
        }

        public void BeginReceive()
        {
            Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(AsyncReceiveCallback), null);
        }

        private void AsyncReceiveCallback(IAsyncResult result)
        {
            try
            {
                SocketError Error;
                int Size = Connection.EndReceive(result, out Error);
                if (Size <= 0)
                {
                    Server.Disconnect(this);
                }
                else
                {
                    if (Cipher != null)
                        Cipher.Decrypt(Buffer, Size);

                    if (Server.OnClientReceived != null)
                        Server.OnClientReceived(this, Buffer, Size);

                    BeginReceive();
                }
            }
            catch (Exception exception)
            {
#if DEBUG
                Console.WriteLine(exception.ToString());
#endif
                Server.Disconnect(this);
            }
        }

        public void Disconnect()
        {
            Server.Disconnect(this);
        }

        public void Dispose()
        {
            if (Cipher != null)
                Cipher.Dispose();
            if (Connection != null)
                Connection.Dispose();
        }
    }
}