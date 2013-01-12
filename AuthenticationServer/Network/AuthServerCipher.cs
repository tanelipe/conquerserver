using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary;
namespace AuthenticationServer
{

    public class AuthCryptography : IPacketCipher
    {
        private byte[] KeyOne;
        private byte[] KeyTwo;
        private byte[] KeyThree;
        private byte[] KeyFour;
        private ushort DecryptCounter;
        private ushort EncryptCounter;

        private bool UsingAlternate;


        public void Encrypt(byte[] Packet)
        {
            lock (this)
            {
                for (int i = 0; i < Packet.Length; i++)
                {
                    Packet[i] = (byte)(Packet[i] ^ 0xAB);
                    Packet[i] = (byte)((Packet[i] << 4) | (Packet[i] >> 4));
                    if (UsingAlternate)
                    {

                        Packet[i] = (byte)(KeyFour[EncryptCounter >> 8] ^ Packet[i]);
                        Packet[i] = (byte)(KeyThree[EncryptCounter & 0xFF] ^ Packet[i]);
                    }
                    else
                    {
                        Packet[i] = (byte)(KeyTwo[EncryptCounter >> 8] ^ Packet[i]);
                        Packet[i] = (byte)(KeyOne[EncryptCounter & 0xFF] ^ Packet[i]);
                    }
                    EncryptCounter++;
                }
            }
        }
        public void Decrypt(byte[] Packet)
        {
            lock (this)
            {
                for (int i = 0; i < Packet.Length; i++)
                {
                    Packet[i] = (byte)(Packet[i] ^ 0xAB);
                    Packet[i] = (byte)((Packet[i] << 4) | (Packet[i] >> 4));
                    if (UsingAlternate)
                    {
                        Packet[i] = (byte)(KeyFour[DecryptCounter >> 8] ^ Packet[i]);
                        Packet[i] = (byte)(KeyThree[DecryptCounter & 0xFF] ^ Packet[i]);
                    }
                    else
                    {
                        Packet[i] = (byte)(KeyTwo[DecryptCounter >> 8] ^ Packet[i]);
                        Packet[i] = (byte)(KeyOne[DecryptCounter & 0xFF] ^ Packet[i]);
                    }
                    DecryptCounter++;
                }
            }
        }

        public void Initialize()
        {
            KeyOne = new byte[256];
            KeyTwo = new byte[256];
            byte iKeyOne = 0x9D;
            byte iKeyTwo = 0x62;
            for (ushort i = 0; i < 256; i++)
            {
                KeyOne[i] = iKeyOne;
                KeyTwo[i] = iKeyTwo;
                iKeyOne = (byte)((0x0F + (byte)(iKeyOne * 0xFA)) * iKeyOne + 0x13);
                iKeyTwo = (byte)((0x79 - (byte)(iKeyTwo * 0x5C)) * iKeyTwo + 0x6D);
            }
            DecryptCounter = 0;
            EncryptCounter = 0;
            UsingAlternate = false;
        }

        public unsafe void GenerateKeys(uint Key1, uint Key2)
        {
            uint dwKey1 = ((Key1 + Key2) ^ 0x4321) ^ Key1;
            uint dwKey2 = (uint)(dwKey1 * dwKey1);
            KeyThree = new byte[256];
            KeyFour = new byte[256];
            fixed (void* uKey1 = KeyOne, uKey3 = KeyThree, uKey2 = KeyTwo, uKey4 = KeyFour)
            {
                for (sbyte i = 0; i < 64; i++)
                {
                    *(((uint*)uKey3) + i) = dwKey1 ^ *(((uint*)uKey1) + i);
                    *(((uint*)uKey4) + i) = dwKey2 ^ *(((uint*)uKey2) + i);
                }
            }
            UsingAlternate = true;
            EncryptCounter = 0;
        }

        public void Dispose()
        {
            
        }
    }
}
