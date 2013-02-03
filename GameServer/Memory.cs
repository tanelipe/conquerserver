
namespace GameServer
{
    using System;
    using System.Runtime.InteropServices;

    public unsafe class Memory
    {
        public static void* Alloc(int size)
        {
            IntPtr heap = Marshal.AllocHGlobal(size);
            for (int i = 0; i < size; i++)
            {
                Marshal.WriteByte(heap, i, 0);
            }
            if (heap == IntPtr.Zero) throw new OutOfMemoryException();
            return heap.ToPointer();
        }
        public static void Copy(void* src, void* dst, int count)
        {
            byte* ps = (byte*)src;
            byte* pd = (byte*)dst;
            if (ps > pd)
            {
                for (; count != 0; count--) *pd++ = *ps++;
            }
            else if (ps < pd)
            {
                for (ps += count, pd += count; count != 0; count--) *--pd = *--ps;
            }
        }
        public static void Free(void* block)
        {
           
            IntPtr Block = new IntPtr(block);
            Marshal.FreeHGlobal(Block);
        }
        public static void* ReAlloc(void* block, int size)
        {
            IntPtr Block = new IntPtr(block);
            IntPtr ReAlloc = Marshal.ReAllocHGlobal(Block, new IntPtr(size));
            for (int i = 0; i < size; i++)
            {
                Marshal.WriteByte(ReAlloc, i, 0);
            }
            return ReAlloc.ToPointer();      
        }
    }
}
