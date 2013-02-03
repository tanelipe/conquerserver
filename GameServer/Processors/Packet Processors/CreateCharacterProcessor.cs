using GameServer.Database;
namespace GameServer
{
    public unsafe class CreateCharacterProcessor : IPacketProcessor
    {
        public CreateCharacterProcessor(DatabaseManager Database) :
            base(Database)
        {

        }

        public override void Execute(GameClient Client, byte* pPacket)
        {
            CharacterCreation* Packet = (CharacterCreation*)pPacket;

            string Username = new string(Packet->Account, 0, 16).Trim('\x00');
            string Name = new string(Packet->Name, 0, 16).Trim('\x00');
            string Password = new string(Packet->Password, 0, 16).Trim('\x00');

            Database.CreateCharacter(Client, Packet->Model, Packet->Class, Name);

            Client.Disconnect();
        }
    }
}
