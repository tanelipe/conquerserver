using GameServer.Database;
namespace GameServer
{
    public unsafe class MovementProcessor : IPacketProcessor
    {
        public MovementProcessor(DatabaseManager Database)
            : base(Database)
        {

        }

        public override void Execute(GameClient Client, byte* pPacket)
        {
            EntityMovement* Packet = (EntityMovement*)pPacket;
            if (Packet->UID == Client.Entity.UID)
            {
                ConquerAngle Direction = (ConquerAngle)(Packet->Direction % 8);
                Client.Entity.Walk(Direction);
                if (!Kernel.IsWalkable(Client.Entity.Location.MapID,
                        Client.Entity.Location.X, Client.Entity.Location.Y))
                {
                    Client.Disconnect();
                }
                else
                {
                    Client.SendScreen(Packet, Packet->Size, true);
                    Kernel.GetScreen(Client, null);
                }
            }
            else
            {
                Client.Disconnect();
            }
        }
    }
}
