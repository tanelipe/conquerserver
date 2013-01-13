
using NetworkLibrary;
namespace AuthenticationServer
{
    public class AuthServerSocket : ServerSocket
    {   

        public AuthServerSocket()
            : base(9958, 10, 8192)
        {

            
            
        }

        public override IPacketCipher CreatePacketCipher()
        {
            return null;
        }
    }
}
