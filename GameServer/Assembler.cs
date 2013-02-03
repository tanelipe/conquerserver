
namespace GameServer
{
    public class Assembler
    {
        public static int RollLeft(uint Value, byte Roll, byte Size)
        {
            Roll = (byte)(Roll & 0x1f);
            return (int)((Value << Roll) | (Value >> (Size - Roll)));
        }

        public static int RollRight(uint Value, byte Roll, byte Size)
        {
            Roll = (byte)(Roll & 0x1f);
            return (int)((Value << (Size - Roll)) | (Value >> Roll));
        }
    }
}
