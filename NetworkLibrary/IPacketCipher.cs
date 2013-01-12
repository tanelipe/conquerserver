namespace NetworkLibrary
{
    public interface IPacketCipher
    {
        void Initialize();
        void Decrypt(byte[] Data);
        void Encrypt(byte[] Data);
        void GenerateKeys(uint Key1, uint Key2);

        void Dispose();
    }
}
