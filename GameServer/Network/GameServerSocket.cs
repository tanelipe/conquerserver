using NetworkLibrary;
namespace GameServer
{
    public class GameServerSocket : ServerSocket
    {

        public GameServerSocket()
            : base(5816, 10, 8192)
        {

        }

        public override IPacketCipher CreatePacketCipher()
        {
            return null;
        }
    }
}
