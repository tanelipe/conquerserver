using GameServer.Database;
namespace GameServer
{
    public unsafe class AttackProcessor : IPacketProcessor
    {
        public AttackProcessor(DatabaseManager Database)
            : base(Database)
        {

        }

        public override void Execute(GameClient Client, byte* pPacket)
        {
            AttackRequest* Request = (AttackRequest*)pPacket;
            if (Request->AttackType == AttackTypes.Magic)
            {
                Request->Decrypt();
                if (Request->SpellID == 1045)
                {
                    AttackTargetPacket* Packet = PacketHelper.AttackPacket();
                    Packet->UID = Client.Entity.UID;
                    Packet->SpellID = 1045;
                    Packet->SpellLevel = 4;
                    Packet->X = Request->TargetX;
                    Packet->Y = Request->TargetY;
                    Client.SendScreen(Packet, Packet->Size, true);
                    Memory.Free(Packet);
                }
            }
        }
    }
}
