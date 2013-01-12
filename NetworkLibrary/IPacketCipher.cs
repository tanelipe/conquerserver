namespace NetworkLibrary
{
    public interface IPacketCipher
    {
        void Decrypt(byte[] Data, int Size);
        void Encrypt(byte[] Data, int Size);
        void Dispose();
    }
}
