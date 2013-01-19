using System;
namespace NetworkLibrary
{
    using System.Net.Sockets;

    public class WinsockClient : IDisposable
    {
        private ServerSocket Server;
        private Socket Connection;
        private byte[] Buffer;
        private bool Disposed;
       
        public object Wrapper { get; set; }

        public WinsockClient(ServerSocket Server, Socket Connection, int BufferSize, IPacketCipher Cipher)
        {
            this.Server = Server;
            this.Connection = Connection;
            Buffer = new byte[BufferSize];
            Disposed = false;
        }

        public void Send(byte[] Packet)
        {
            try
            {
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
            if (Disposed) return;

            Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(AsyncReceiveCallback), null);
        }

        private void AsyncReceiveCallback(IAsyncResult result)
        {
            if (Disposed) return;

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
                    byte[] Packet = new byte[Size];
                    System.Buffer.BlockCopy(Buffer, 0, Packet, 0, Size);

                    if (Server.OnClientReceived != null)
                    {
                        Server.OnClientReceived.BeginInvoke(this, Packet, Size, null, null);
                    }
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
            if (Server.OnClientDisconnected != null)
                Server.OnClientDisconnected(this, null);
            Dispose();
        }

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
            Disposed = true;
        }
    }
}