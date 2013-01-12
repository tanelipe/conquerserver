using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AuthenticationServer
{
    unsafe class PasswordCipher
    {
        static int RollLeft(uint Value, byte Roll, byte Size)
        {
            return (int)((Value << Roll) | (Value >> (Size - Roll)));
        }
        static int RollRight(uint Value, byte Roll, byte Size)
        {
            return (int)((Value << (Size - Roll)) | (Value >> Roll));
        }
        public static uint[] PasswordKey = new uint[] 
        { 
            0xebe854bc, 0xb04998f7, 0xfffaa88c, 0x96e854bb, 0xa9915556,
            0x48e44110, 0x9f32308f, 0x27f41d3e, 0xcf4f3523, 0xeac3c6b4,
            0xe9ea5e03, 0xe5974bba, 0x334d7692, 0x2c6bcf2e, 0xdc53b74,
            0x995c92a6, 0x7e4f6d77, 0x1eb2b79f, 0x1d348d89, 0xed641354,
            0x15e04a9d, 0x488da159, 0x647817d3, 0x8ca0bc20, 0x9264f7fe,
            0x91e78c6c, 0x5c9a07fb, 0xabd4dcce, 0x6416f98d, 0x6642ab5b
        };
        public static string Decrypt(uint* Password)
        {
            for (sbyte i = 1; i >= 0; i--)
            {
                uint temp1 = Password[(i * 2) + 1];
                uint temp2 = Password[i * 2];
                for (sbyte i2 = 11; i2 >= 0; i2 = (sbyte)(i2 - 1))
                {
                    temp1 = ((uint)RollRight(temp1 - PasswordKey[(i2 * 2) + 7], (byte)temp2, 0x20)) ^ temp2;
                    temp2 = ((uint)RollRight(temp2 - PasswordKey[(i2 * 2) + 6], (byte)temp1, 0x20)) ^ temp1;
                }
                Password[(i * 2) + 1] = temp1 - PasswordKey[5];
                Password[i * 2] = temp2 - PasswordKey[4];
            }
            return new string((sbyte*)Password).Trim('\x00');
        }
        public static byte[] Encrypt(string password)
        {
            UInt32 tmp1, tmp2, tmp3, tmp4, A, B, chiperOffset, chiperContent;

            byte[] plain = new byte[16];
            Encoding.ASCII.GetBytes(password, 0, password.Length, plain, 0);

            MemoryStream mStream = new MemoryStream(plain);
            BinaryReader bReader = new BinaryReader(mStream);
            UInt32[] pSeeds = new UInt32[4];
            for (int i = 0; i < 4; i++) pSeeds[i] = bReader.ReadUInt32();
            bReader.Close();

            chiperOffset = 7;

            byte[] encrypted = new byte[plain.Length];
            MemoryStream eStream = new MemoryStream(encrypted);
            BinaryWriter bWriter = new BinaryWriter(eStream);

            for (int j = 0; j < 2; j++)
            {
                tmp1 = tmp2 = tmp3 = tmp4 = 0;
                tmp1 = PasswordKey[5];
                tmp2 = pSeeds[j * 2];
                tmp3 = PasswordKey[4];
                tmp4 = pSeeds[j * 2 + 1];

                tmp2 += tmp3;
                tmp1 += tmp4;

                A = B = 0;

                for (int i = 0; i < 12; i++)
                {
                    chiperContent = 0;
                    A = (uint)RollLeft(tmp1 ^ tmp2, (byte)tmp1, 32);
                    chiperContent = PasswordKey[chiperOffset + i * 2 - 1];
                    tmp2 = A + chiperContent;

                    B = (uint)RollLeft(tmp1 ^ tmp2, (byte)tmp2, 32);
                    chiperContent = PasswordKey[chiperOffset + i * 2];
                    tmp1 = B + chiperContent;
                }

                bWriter.Write(tmp2);
                bWriter.Write(tmp1);
            }
            bWriter.Close();

            return encrypted;
        }
    }
}

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albetros.Encryption
{

    public sealed class RC5Exception : Exception
    {
        public RC5Exception(string message) : base(message) { }
    }

    public sealed class RC5
    {
        
    }
}



*/