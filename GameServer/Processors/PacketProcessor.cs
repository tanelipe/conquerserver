using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Database;
namespace GameServer.Processors
{
    public unsafe class PacketProcessor
    {
        private CommandProcessor CommandProcessor;
        private DatabaseManager Database;

        public PacketProcessor(DatabaseManager Database)
        {
            this.Database = Database;
            CommandProcessor = new CommandProcessor();
        }

        public void Process(GameClient Client, byte[] Packet)
        {          
            fixed (byte* pPacket = Packet)
            {
                ushort *Size = (ushort*)(pPacket + 0);
                ushort* Type = (ushort*)(pPacket + 2);

                if (*Size != Packet.Length)
                {
                    SizeMismatch(Client, *Size, Packet);
                    return;
                }

                Kernel.HexDump(Packet, "Client -> Server");

                switch (*Type)
                {
                    case 0x3E9: HandleCharacterCreation(Client, pPacket); break;
                    case 0x3EC: HandleChatting(Client, pPacket); break;
                    case 0x3F1: HandleItemUsage(Client, pPacket); break;
                    case 0x3F2: HandleGeneralData(Client, pPacket); break;
                    case 0x41C: HandleTransfer(Client, pPacket); break;
                }
            }
        }
        private void SizeMismatch(GameClient Client, ushort Size, byte[] Packet)
        {
            int RemainingSize = Packet.Length - Size;
            if (RemainingSize > 0)
            {                
                byte[] Header = new byte[Size];
                Buffer.BlockCopy(Packet, 0, Header, 0, Size);

                byte[] Footer = new byte[RemainingSize];
                Buffer.BlockCopy(Packet, Size, Footer, 0, RemainingSize);

                Process(Client, Header);
                Process(Client, Footer);
            }
        }

        private void HandleChatting(GameClient Client, byte* pPacket)
        {
            Chat* Packet = (Chat*)pPacket;
            string[] Parameters = PacketHelper.ParseChat(Packet);

            if (CommandProcessor.Process(Client, Parameters))
                return;
            


        }
        private unsafe void HandleItemUsage(GameClient Client, byte* pPacket)
        {
            ItemUsage* Packet = (ItemUsage*)pPacket;
            switch (Packet->UsageID)
            {
                case ItemUsageIDs.Ping: Client.Send(Packet, Packet->Size); break;
            }
        }
        private unsafe void HandleGeneralData(GameClient Client, byte* pPacket)
        {
            GeneralData* Packet = (GeneralData*)pPacket;
            switch (Packet->DataID)
            {
                case GeneralDataID.SetLocation:
                    {
                        Packet->ValueA = Client.Entity.Location.X;
                        Packet->ValueB = Client.Entity.Location.Y;
                        Packet->ValueD_High = Client.Entity.Location.MapID;
                        Client.Send(Packet, Packet->Size);
                    } break;
                case GeneralDataID.Jump:
                    {
                        ushort X1 = Packet->ValueA;
                        ushort Y1 = Packet->ValueB;

                        if ((X1 != Client.Entity.Location.X) || (Y1 != Client.Entity.Location.Y))
                        {
                            //TODO: Jump hack prevention
                            Client.Disconnect();
                        }
                        else
                        {
                            ushort X2 = Packet->ValueD_High;
                            ushort Y2 = Packet->ValueD_Low;

                            Client.Entity.Location.X = X2;
                            Client.Entity.Location.Y = Y2;
                        }                       
                    } break;
            }
        }
        private unsafe void HandleCharacterCreation(GameClient Client, byte* pPacket)
        {
            CharacterCreation* Packet = (CharacterCreation*)pPacket;

            string Username = new string(Packet->Account, 0, 16).Trim('\x00');
            string Name = new string(Packet->Name, 0, 16).Trim('\x00');
            string Password = new string(Packet->Password, 0, 16).Trim('\x00');


            Database.CreateCharacter(Client, Packet->Model, Packet->Class, Name);
            
            Client.Disconnect();
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
                Client.UID = Message->AccountID;

                Client.GenerateKeys(Message->LoginToken, Message->AccountID);
                if (Database.GetCharacterData(Client))
                {
                    Chat* Response = PacketHelper.CreateChat("SYSTEM", "ALLUSERS", "ANSWER_OK");
                    Response->ChatType = ChatType.LoginInformation;
                    Response->ID = Message->AccountID;
                    Client.Send(Response, Response->Size);
                    Memory.Free(Response);  

                    CharacterInformation* Information = PacketHelper.CreateInformation(Client);
                    Client.Send(Information, Information->Size);
                    Memory.Free(Information);     
                }
                else
                {
                    Chat* Response = PacketHelper.CreateChat("SYSTEM", "ALLUSERS", "NEW_ROLE");
                    Response->ChatType = ChatType.LoginInformation;
                    Response->ID = Message->AccountID;
                    Client.Send(Response, Response->Size);
                    Memory.Free(Response);
                }

            }
            else
            {
                Client.Disconnect();
            }
        }
       
    }
}
