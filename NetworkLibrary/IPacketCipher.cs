namespace NetworkLibrary
{
    public interface IPacketCipher
    {
        void Initialize();
        void Decrypt(byte[] Data, int Size);
        void Encrypt(byte[] Data, int Size);
        void GenerateKeys(uint Key1, uint Key2);

        void Dispose();
    }
}
