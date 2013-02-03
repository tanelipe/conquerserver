using GameServer.Database;
using GameServer.Scripting;
namespace GameServer
{
    public unsafe class NpcProcessor : IPacketProcessor
    {
        private NpcScripting NpcScriptEngine;
        public NpcProcessor(DatabaseManager Database, NpcScripting NpcScriptEngine) :
            base(Database)
        {
            this.NpcScriptEngine = NpcScriptEngine;
        }
        public override void Execute(GameClient Client, byte* pPacket)
        {
            PacketHeader* Header = (PacketHeader*)pPacket;
            if (Header->Type == 0x7EF)
            {
                NpcInitialize* Packet = (NpcInitialize*)pPacket;
                NpcScriptEngine.Initialize(Client, Packet->UID);
            }
            else
            {
                NpcDialog* Packet = (NpcDialog*)pPacket;
                if (Packet->OptionID != 255)
                {
                    string Input = new string(Packet->Input, 1, Packet->Input[0]);
                    NpcScriptEngine.Process(Client, Packet->OptionID, Input);
                }
                else
                {
                    Client.ActiveNPC = 0;
                }
            }
        }
    }
}
