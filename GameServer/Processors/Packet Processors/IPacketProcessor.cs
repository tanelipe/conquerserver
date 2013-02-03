using GameServer.Database;
namespace GameServer
{
    public unsafe abstract class IPacketProcessor
    {
        protected DatabaseManager Database;
        public IPacketProcessor(DatabaseManager Database)
        {
            this.Database = Database;
        }

        public abstract void Execute(GameClient Client, byte* pPacket);
    }
}
