using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Processors
{
    public unsafe class PacketProcessor
    {
        public void Process(GameClient Client, byte[] Packet)
        {
            Client.Decrypt(Packet);
            HexDump(Packet, "Client -> Server");
            
            fixed (byte* pPacket = Packet)
            {
                ushort *Size = (ushort*)(pPacket + 0);
                ushort* Type = (ushort*)(pPacket + 2);

                switch (*Type)
                {
                    case 0x41C: HandleTransfer(Client, pPacket); break;
                }
            }
        }
        private unsafe void HandleTransfer(GameClient Client, byte* Packet)
        {
            AuthMessage* Message = (AuthMessage*)Packet;
            uint Token = Message->AccountID | 0xAABB;
            Token = Message->AccountID << 8 | Message->AccountID;
            Token = Token ^ 0x4321;
            Token = Token << 8 | Token;

            if (Token == Message->LoginToken)
            {

            }
            else
            {
                Client.Disconnect();
            }

            /*
            Response->AccountID = Client.GetAccountID();

            uint Token = Response->AccountID | 0xAABB;
            Token = Response->AccountID << 8 | Response->AccountID;
            Token ^= 0x4321;
            Token = Token << 8 | Token;
            Response->LoginToken = Token;

             * */
        }
        private unsafe void HexDump(byte[] Packet, string Header)
        {
            StringBuilder dump = new StringBuilder();

            fixed (byte* pPacket = Packet)
            {
                ushort Size = *(ushort*)pPacket;
                ushort Type = *(ushort*)(pPacket + 2);

                dump.AppendLine(string.Format("[{0} Size: 0x{1:X4} Type: 0x{2:X4}]", Header, Size, Type));
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
