using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary;
using AuthenticationServer.Database;
namespace AuthenticationServer
{
    public class Program
    {
        private static DatabaseManager Database;
        static void Main(string[] args)
        {
            AuthServerSocket Server = new AuthServerSocket();
            Server.OnClientConnected = OnClientConnect;
            Server.OnClientDisconnected = OnClientDisconnect;
            Server.OnClientReceived = OnClientReceive;
            Server.Initialize();

            Database = new DatabaseManager();
  

            bool Running = true;
            while (Running)
            {
               
            }
        }
        private static bool CheckCommand(string Command, string Input)
        {
            return Command.ToLower() == Input.ToLower();
        }
        private static void OnClientConnect(WinsockClient Socket, object NullParam)
        {
            Socket.Wrapper = new AuthClient(Socket);
        }
        private static void OnClientDisconnect(WinsockClient Socket, object NullParam)
        {

        }
        private static unsafe void OnClientReceive(WinsockClient Socket, byte[] Packet, int Length)
        {
            AuthClient Client = Socket.Wrapper as AuthClient;
            Client.Decrypt(Packet);

            fixed (byte* pPacket = Packet)
            {
                ushort Size = *(ushort*)pPacket;
                ushort Type = *(ushort*)(pPacket + 2);
                HexDump(Packet, "Client -> Server", Size, Type);


                switch (Type)
                {
                    case 0x41B:
                        {
                            AuthRequest* Request = (AuthRequest*)pPacket;
                            string Username = new string(Request->Username, 0, 16).Trim('\x00');
                            string Password = PasswordCipher.Decrypt((uint*)Request->Password);
                            string Server = new string(Request->Server, 0, 16).Trim('\x00');

                            string Address = "";
                            if (Database.ServerExists(Server, out Address))
                            {
                                if (Database.AccountExists(Username, Password, Client))
                                {
                                    SendAuthResponse(Client, Address, 5816);
                                }
                                else
                                {
                                    SendAuthReject(Client, InvalidCredentials());
                                    //Client.Disconnect();
                                }
                            }

                        } break;
                }
            }
        }

        private static unsafe void SendAuthResponse(AuthClient Client, string Address, ushort Port)
        {
            AuthResponse* Response = stackalloc AuthResponse[1];
            Response->Size = (ushort)sizeof(AuthResponse);
            Response->Type = 0x41F;
            Response->AccountID = Client.GetAccountID();

            uint Token = Response->AccountID | 0xAABB;
            Token = Response->AccountID << 8 | Response->AccountID;
            Token ^= 0x4321;
            Token = Token << 8 | Token;
            Response->LoginToken = Token;

            Response->Port = Port;
            for (int i = 0; i < 16; i++)
            {
                if (i >= Address.Length)
                    Response->Address[i] = 0;
                else
                    Response->Address[i] = (sbyte)Address[i];
            }
            Client.Send(Response, Response->Size);
        }


        public static byte[] InvalidCredentials()
        {
            return new byte[] {
                0xD5, 0xCA, 0xBA, 0xC5, 0xC3, 0xFB, 0xBB, 0xF2, 0xBF, 0xDA, 0xC1, 0xEE, 0xB4, 0xED         
            };
        }
        public static byte[] ServerDown()
        {
            return new byte[] {
                0xB7, 0xFE, 0xCE, 0xF1, 0xC6, 0xF7, 0xCE, 0xB4, 0xC6, 0xF4, 0xB6, 0xAF, 0x00, 0x00
            };
        }


        private static unsafe void SendAuthReject(AuthClient Client, byte[] Payload)
        {
            AuthResponse* Response = stackalloc AuthResponse[1];
            Response->Size = (ushort)sizeof(AuthResponse);
            Response->Type = 0x41F;
            Response->AccountID = 0;
            Response->LoginToken = 1;
            for (int i = 0; i < 16; i++)
            {
                if (i >= Payload.Length)
                    Response->Address[i] = 0;
                else
                    Response->Address[i] = (sbyte)Payload[i];
            }
            Client.Send(Response, Response->Size);
        }

        private static unsafe void HexDump(void* Packet, string Header, ushort Size, ushort Type)
        {
            byte[] Dump = new byte[Size];
            byte* pPacket = (byte*)Packet;
            for (int i = 0; i < Size; i++)
                Dump[i] = pPacket[i];
            HexDump(Dump, Header, Size, Type);
        }

        private static unsafe void HexDump(byte[] Packet, string Header, ushort Size, ushort Type)
        {
            StringBuilder dump = new StringBuilder();

            fixed (byte* pPacket = Packet)
            {

                dump.AppendLine(string.Format("[{0} Size: {1} Type: {2}]", Header, Size.ToString("X4"), Type.ToString("X4")));
                for (int i = 0; i < Packet.Length; i += 16)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (i + j < Packet.Length)
                            dump.AppendFormat("{0:X2} ", Packet[i + j]);
                        else
                            dump.Append("   ");
                    }
                    dump.Append(" ; ");
                    for (int j = 0; j < 16; j++)
                    {
                        if (i + j < Packet.Length)
                        {
                            char Value = (char)Packet[i + j];
                            if (char.IsLetterOrDigit(Value))
                                dump.Append(Value);
                            else
                                dump.Append(".");
                        }
                    }
                    dump.AppendLine();
                }
            }
            Console.WriteLine(dump.ToString());
        }

    }
}
