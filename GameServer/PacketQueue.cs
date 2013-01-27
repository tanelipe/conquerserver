using System;
using System.Collections.Generic;

namespace GameServer
{
    public class PacketQueue
    {
        private List<byte> Queue;
        private byte[] Data;

        public PacketQueue()
        {
            Queue = new List<byte>();
        }
        public void Push(byte[] Packet)
        {
            lock (Queue)
            {
                Queue.AddRange(Packet);
                Data = Queue.ToArray();
            }
        }
        public byte[] Pop()
        {
            lock (Queue)
            {
                if (Data.Length < 2) return null;
                ushort Size = BitConverter.ToUInt16(Data, 0);
                if (Size > Data.Length)
                {
                    // Framentation, rest of packet should follow in the next transmission(s)
                    return null;
                }
                else
                {
                    byte[] Packet = new byte[Size];
                    Buffer.BlockCopy(Data, 0, Packet, 0, Size);

                    Queue.RemoveRange(0, Size);
                    Data = Queue.ToArray();

                    return Packet;
                }
            }
        }
        public int Size
        {
            get { return Queue == null ? -1 : Queue.Count; }
        }
    }
}
