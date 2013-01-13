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
        private DatabaseManager Database;

        public PacketProcessor(DatabaseManager Database)
        {
            this.Database = Database;
        }

        public void Process(GameClient Client, byte[] Packet)
        {
            Client.Decrypt(Packet);
            Kernel.HexDump(Packet, "Client -> Server");
            
            fixed (byte* pPacket = Packet)
            {
                ushort *Size = (ushort*)(pPacket + 0);
                ushort* Type = (ushort*)(pPacket + 2);

                switch (*Type)
                {
                    case 0x3E9: HandleCharacterCreation(Client, pPacket); break;
                    case 0x3F2: HandleGeneralData(Client, pPacket); break;
                    case 0x41C: HandleTransfer(Client, pPacket); break;
                }
            }
        }
        private unsafe void HandleGeneralData(GameClient Client, byte* pPacket)
        {
            GeneralData* Packet = (GeneralData*)pPacket;
            switch (Packet->DataID)
            {
                case GeneralDataID.SetLocation:
                    Packet->ValueA = 400;
                    Packet->ValueB = 400;
                    Packet->ValueD_High = 1002;
                    Kernel.HexDump(Packet, Packet->Size, "SETLOCATION");
                    //Client.Send(Packet, Packet->Size);
                    break;
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

              
                if (Database.GetCharacterData(Client))
                {
                    CharacterInformation* Information = PacketConstructor.CreateInformation(Client);
                    Client.Send(Information, Information->Size);
                    Memory.Free(Information);

     
                    Chat* Response = PacketConstructor.CreateChat("SYSTEM", "ALLUSERS", "ANSWER_OK");
                    Response->ChatType = ChatType.LoginInformation;
                    Response->ID = Message->AccountID;

                    Client.Send(Response, Response->Size);
                    Memory.Free(Response);      
                }
                else
                {
                    Chat* Response = PacketConstructor.CreateChat("SYSTEM", "ALLUSERS", "NEW_ROLE");
                    Response->ChatType = ChatType.LoginInformation;
                    Response->ID = Message->AccountID;
                    Client.Send(Response, Response->Size);
                    Memory.Free(Response);
                }
                Client.GenerateKeys(Message->LoginToken, Message->AccountID);
            }
            else
            {
                Client.Disconnect();
            }
        }
       
    }
}
