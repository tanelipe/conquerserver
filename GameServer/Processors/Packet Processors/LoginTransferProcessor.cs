using GameServer.Database;
namespace GameServer
{
    public unsafe class LoginTransferProcessor : IPacketProcessor
    {
        public LoginTransferProcessor(DatabaseManager Database)
            : base(Database)
        {

        }

        public override void Execute(GameClient Client, byte* pPacket)
        {
            AuthMessage* Message = (AuthMessage*)pPacket;
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

                    EntityManager.Add(Client);
                    Client.Status = LoginStatus.Complete;
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
