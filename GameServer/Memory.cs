
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
            return ReAlloc.ToPointer();      
        }
        // Heap API flags
        const int HEAP_ZERO_MEMORY = 0x00000008;
        // Heap API functions
        [DllImport("kernel32")]
        static extern int GetProcessHeap();
        [DllImport("kernel32")]
        static extern void* HeapAlloc(int hHeap, int flags, int size);
        [DllImport("kernel32")]
        static extern bool HeapFree(int hHeap, int flags, void* block);
        [DllImport("kernel32")]
        static extern void* HeapReAlloc(int hHeap, int flags,
           void* block, int size);
        [DllImport("kernel32")]
        static extern int HeapSize(int hHeap, int flags, void* block);
        [DllImport("Kernel32")]
        static extern int GetLastError();
    }
}
