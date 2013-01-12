using System;
namespace NetworkLibrary
{
    using System.Net;
    using System.Net.Sockets;

    public abstract class ServerSocket
    {
        private Socket Connection;
        private int BufferSize;
        private int Backlog;
        private int Port;


        public NetworkEvent<WinsockClient, object> OnClientConnected;
        public NetworkEvent<WinsockClient, object> OnClientDisconnected;
        public NetworkEvent<WinsockClient, byte[], int> OnClientReceived;

        public abstract IPacketCipher CreatePacketCipher();

        public ServerSocket(int Port, int Backlog, int BufferSize)
        {
            this.Port = Port;
            this.Backlog = Backlog;
            this.BufferSize = BufferSize;

        }
        public void Initialize()
        {
            try
            {
                Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint EndPoint = new IPEndPoint(IPAddress.Any, Port);

                Connection.Bind(EndPoint);
                Connection.Listen(Backlog);

                Connection.BeginAccept(new AsyncCallback(AcceptConnectionCallback), null);
            }
            catch (Exception exception)
            {
#if DEBUG
                Console.WriteLine(exception.ToString());
#endif
            }
        }


        private void AcceptConnectionCallback(IAsyncResult result)
        {
            try
            {
                WinsockClient Client = new WinsockClient(this, Connection.EndAccept(result), BufferSize, CreatePacketCipher());
                if (OnClientConnected != null)
                    OnClientConnected(Client, null);

                Client.BeginReceive();
                Connection.BeginAccept(new AsyncCallback(AcceptConnectionCallback), null);
            }
            catch (Exception exception)
            {
#if DEBUG
                Console.WriteLine(exception.ToString());
#endif
            }
        }


        public void Disconnect(WinsockClient Client)
        {
            if (OnClientDisconnected != null)
                OnClientDisconnected(Client, null);

            Client.Dispose();
        }
    }
}